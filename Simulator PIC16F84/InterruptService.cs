using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class InterruptService
    {
        private RegisterByte intCon;
        private RegisterByte portB;
        private RegisterByte option;
        private RegisterByte eeCon1;
        private RegisterByte trisB;
        private Stack stack;
        private ProgramCounter PC;
        private byte intInterruptOldValue;
        private byte portBInterruptOldValue;


        public InterruptService(RegisterByte intCon, RegisterByte portB, RegisterByte option, RegisterByte eeCon1, RegisterByte trisB, Stack stack, ProgramCounter PC)
        {
            this.intCon = intCon;
            this.portB = portB;
            this.option = option;
            this.eeCon1 = eeCon1;
            this.trisB = trisB;
            this.stack = stack;
            this.PC = PC;
            this.intInterruptOldValue = 0;
            this.portBInterruptOldValue = 0;
        }

        public void executeRoutine()
        {
            if (!isGlobalInterruptEnableBitSet())
                return;

            if (isThereAnInterruptRequest())
                executeInterruptServiceRoutine();
        }

        private void executeInterruptServiceRoutine()
        {
            clearGlobalInterruptEnableBit();
            stack.PushOntoStack(new ProgramMemoryAddress(PC.DeriveReturnAddress()));
            PC.Counter.Address = 0x04 - 1;
        }

        private bool isGlobalInterruptEnableBitSet()
        {
            return intCon.isBitSet(7);
        }

        private bool isThereAnInterruptRequest()
        {
            return (isThereAnIntInterruptRequest() || isThereATimer0InterruptRequest() || isThereAPortBInterruptRequest() || isThereAPDataEEPROMInterruptRequest());
        }

        private bool isThereAnIntInterruptRequest()
        {
            return (isIntInterruptFlagBitSet() && isIntInterruptEnabled());
        }

        private bool isThereATimer0InterruptRequest()
        {
            return (isTimer0InterruptFlagSet() && isTimer0OverflowInterruptEnabled());
        }

        private bool isThereAPortBInterruptRequest()
        {
            return (isRBInterruptEnabled() && isRBInterruptFlagSet());
        }

        private bool isThereAPDataEEPROMInterruptRequest()
        {
            return (isEEPROMInterruptEnabled() && isEEPROMInterruptFlagSet());
        }

        private bool isEEPROMInterruptFlagSet()
        {
            return eeCon1.isBitSet(4);
        }

        private bool isTimer0InterruptFlagSet()
        {
            return intCon.isBitSet(2);
        }

        private bool isEEPROMInterruptEnabled()
        {
            return intCon.isBitSet(6);
        }

        private bool isTimer0OverflowInterruptEnabled()
        {
            return intCon.isBitSet(5);
        }

        private bool isIntInterruptEnabled()
        {
            return intCon.isBitSet(4);
        }

        private bool isIntEdgBitSet()
        {
            return option.isBitSet(6);
        }

        private bool isIntInterruptFlagBitSet()
        {
            return intCon.isBitSet(1);
        }

        private bool isRBInterruptEnabled()
        {
            return intCon.isBitSet(3);
        }

        private bool isRBInterruptFlagSet()
        {
            return intCon.isBitSet(0);
        }

        private void setRBInterruptFlag()
        {
            intCon.setBit(0);
        }

        public void setGlobalInterruptEnableBit()
        {
            intCon.setBit(7);
        }

        private void clearGlobalInterruptEnableBit()
        {
            intCon.clearBit(7);
        }

        /// <summary>
        /// INT INTERRUPT
        /// 
        /// External interrupt on RB0/INT pin is edge triggered:
        /// either rising if INTEDG bit (OPTION_REG<6>) is set,
        /// or falling if INTEDG bit is clear.
        /// When a valid edge
        /// appears on the RB0/INT pin, the INTF bit
        /// (INTCON<1>) is set. This interrupt can be disabled by
        /// clearing control bit INTE (INTCON<4>).
        /// </summary>
        public void checkForIntInterrupt()
        {
            if (isIntInterruptEnabled() && isGlobalInterruptEnableBitSet())
            {
                if (isIntEdgBitSet())
                {
                    if (portB.checkForRisingEdge(intInterruptOldValue, 0))
                        setIntFBit();
                }
                else
                {
                    if (portB.checkForFallingEdge(intInterruptOldValue, 0))
                        setIntFBit();
                }
            }
            intInterruptOldValue = portB.Value;
        }

        /// <summary>
        /// PORTB INTERRUPT
        /// 
        /// An input change on PORTB<7:4> sets flag bit RBIF
        /// (INTCON<0>). The interrupt can be enabled/disabled
        /// by setting/clearing enable bit RBIE (INTCON<3>)
        /// (Section 4.2).
        /// </summary>
        public void checkForPortBInterrupt()
        {
            if (isGlobalInterruptEnableBitSet() && isRBInterruptEnabled())
                if (checkForPortBInterruptInputChange())
                    setRBInterruptFlag();
        }

        /// <summary>
        /// PORT B INTERRUPT CONDITION
        /// 
        /// The Port B interrupt is triggered by input changes
        /// on PORTB<7:4>.
        /// For Port B to work as input, the corresponding bit
        /// in the TRISB-Register needs to be set to 1.
        /// </summary>
        /// <returns></returns>
        private bool checkForPortBInterruptInputChange()
        {
            byte changedBits = (byte)(portBInterruptOldValue ^ portB.Value);
            byte inputChange = (byte)(changedBits & trisB.Value);
            if ((inputChange & 0xF0) != 0x00)
            {
                portBInterruptOldValue = portB.Value;
                return true;
            }
            return false;
        }

        private void setIntFBit()
        {
            intCon.setBit(1);
        }

        private void clearIntFBit()
        {
            intCon.clearBit(1);
        }
    }
}
