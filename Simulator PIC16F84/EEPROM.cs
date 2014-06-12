using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class EEPROM
    {
        private byte[] EEPROMMemory;
        private RegisterByte EECON1;
        private RegisterByte EECON2;
        private RegisterByte EEADR;
        private RegisterByte EEDATA;
        public event EventHandler EepromChanged;
        private bool eecon2Clear;
        private enum WriteSequence { Idle, Initialized, ListeningForWRBit };
        WriteSequence writeSequenceStatus;

        /// <summary>
        /// DATA EEPROM MEMORY
        /// 
        /// PIC16F84A devices have 64 bytes of
        /// data EEPROM with an address range from 0h to 3Fh.
        /// </summary>
        public EEPROM(RegisterByte EECON1, RegisterByte EECON2, RegisterByte EEADR, RegisterByte EEDATA)
        {
            this.EECON1 = EECON1;
            this.EECON2 = EECON2;
            this.EEADR = EEADR;
            this.EEDATA = EEDATA;
            this.eecon2Clear = false;
            this.writeSequenceStatus = WriteSequence.Idle;
            this.EEPROMMemory = new byte[64];
            for (int var = 0; var < EEPROMMemory.Length; var++)
            {
                EEPROMMemory[var] = new byte();
                EEPROMMemory[var] = 0;
            }
        }

        /// <summary>
        /// EEPROM Functionality:
        /// 
        /// 0. WR bit in EECON1 is inhibited from being set unless the WREN bit is set
        /// 1. Reading the EEPROM Data Memory:
        ///     - Control bit RD (EECON1<0>) needs to be set
        ///     - read address is taken from the EEADR register
        ///     - data avaiable next cycle in EEDATA register
        ///     - RD cleared by hardware
        /// 2. Writing to the EEPROM Data Memory:
        ///     - WREN bit in EECON1 must be set to initiate write sequence, but does not affect sequence if cleared afterwards
        ///     - to initiate write, a specific sequence of steps needs to be followed
        ///         a. write 55h to EECON2
        ///         b. write AAh to EECON2
        ///         c. set WR bit in EECON1
        ///     - Write address is taken from the EEADR register
        ///     - Write data is taken from the EEDATA register
        ///     - after completion of write cycle:
        ///         a. WR bit is cleared in hardware
        ///         b. EE Write Complete Interrupt Flag bit (EEIF) is set
        /// </summary>
        public void checkEEPROMFunctionality()
        {
            // WR bit inhibition
            if ( !isEEPROMWriteEnableBitSet() && isEEPROMWriteControlBitSet())
                clearWriteControlBit();

            // Reading
            if (isEEPROMReadControlBitSet())
            {
                readEEPROM();
                clearEEPROMReadControlBit();
            }

            // Write Sequence Step 1
            if (isEEPROMWriteEnableBitSet() && isWriteSequenceIdle())
            {
                if ( !eecon2Clear && EECON2.Value != 0x55 )
                        eecon2Clear = true;
                if ( eecon2Clear && EECON2.Value == 0x55)
                    initializeWriteSequence();
            }

            // Write Sequence Step 2
            if (isWriteSequenceInitialized())
            {
                if (EECON2.Value == 0xAA)
                {
                    writeSequenceStatus = WriteSequence.ListeningForWRBit;
                }
            }

            // Write Sequence Step 3
            if (isWriteSequenceListeningForWriteControlBit())
            {
                if ( isEEPROMWriteControlBitSet())
                {
                    if( isEEPROMWriteEnableBitSet() )
                        writeToEEPROM();
                    writeCycleCompleted();
                }
            }
        }

        public void clearEEPROM()
        {
            writeSequenceStatus = WriteSequence.Idle;
            eecon2Clear = false;
            for (int var = 0; var < EEPROMMemory.Length; var++)
            {
                EEPROMMemory[var] = 0;
            }
        }

        public byte getEEPROMEntry(int index)
        {
            return EEPROMMemory[index];
        }

        private bool isWriteSequenceListeningForWriteControlBit()
        {
            return writeSequenceStatus == WriteSequence.ListeningForWRBit;
        }

        private bool isWriteSequenceInitialized()
        {
            return writeSequenceStatus == WriteSequence.Initialized;
        }

        private void writeCycleCompleted()
        {
            clearWriteControlBit();
            setEEWriteCompleteInterruptFlag();
            writeSequenceStatus = WriteSequence.Idle;
        }

        private void setEEWriteCompleteInterruptFlag()
        {
            EECON1.setBit(4);
        }

        private void writeToEEPROM()
        {
            EEPROMMemory[EEADR.Value] = EEDATA.Value;
            this.EepromChanged(this, EventArgs.Empty);
        }

        private void clearWriteControlBit()
        {
            EECON1.clearBit(1);
        }

        private bool isEEPROMWriteControlBitSet()
        {
            return EECON1.isBitSet(1);
        }

        private bool isWriteSequenceIdle()
        {
            return writeSequenceStatus == WriteSequence.Idle;
        }

        private bool isEEPROMWriteEnableBitSet()
        {
            return EECON1.isBitSet(2);
        }

        private bool isEEPROMReadControlBitSet()
        {
            return EECON1.isBitSet(0);
        }

        private void clearEEPROMReadControlBit()
        {
            EECON1.clearBit(0);
        }

        public void readEEPROM()
        {
            EEDATA.Value = EEPROMMemory[EEADR.Value];
        }

        private void initializeWriteSequence()
        {
            writeSequenceStatus = WriteSequence.Initialized;
        }


    }
}
