using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterFileMap
    {
        private RegisterByte[] RegisterList;
        public int[] MappingArray;
        //private byte timer;
        private TimerStatus status;
        private int inhibitCycles;

        public byte Timer {
            get
            {
                return RegisterList[0x01].Value;
            }
            set
            {
                inhibitCycles = 2;
                RegisterList[0x01].Value = value;
            }
        }

        public void SetTimerMode()
        {
            status = TimerStatus.TIMER;
            RegisterList[0x81].Value = ClearBit(RegisterList[0x81].Value, 5);
            //Clear Bit 5 in 81h
        }


        public void SetCounterMode()
        {
            status = TimerStatus.COUNTER;
            RegisterList[0x81].Value = SetBit(RegisterList[0x81].Value, 5);
            //Set Bit 5 in 81h
        }

        public void IncrementTimer()
        {
            switch (status)
            {
                case TimerStatus.COUNTER:
                    break;
                case TimerStatus.TIMER:
                    if (inhibitCycles <= 0)
                    {
                        RegisterList[0x01].Value++;
                    }
                    inhibitCycles--;
                    break;
                default:
                    break;
            }
        }

        public void TimerInterrupt()
        {
            RegisterList[0x0b].Value = SetBit(RegisterList[0x0b].Value, 2);
        }

        public void EnableTimerInterrupt()
        {
            RegisterList[0x0b].Value = SetBit(RegisterList[0x0b].Value, 5);
        }

        public void DisableTimerInterrupt()
        {
            RegisterList[0x0b].Value = ClearBit(RegisterList[0x0b].Value, 5);
        }

        public void Overflow(object sender, int index)
        {
            if(index == 1 && IsBitSet(RegisterList[0x0b].Value, 5))
            {
                TimerInterrupt();
            }
        }

        public bool IsBitSet(byte value, int pos)
        {
            return (((value >> pos) & 0x1) == 0x1);
        }

        public RegisterFileMap()
        {
            fillMappingArray();
            RegisterList = new RegisterByte[256];
            for (int var = 0; var < RegisterList.Length; var++ )
            {
                RegisterList[var] = new RegisterByte(var);//TODO muss das var oder 0 sein? new RegisterByte(0)
                RegisterList[var].Overflow += new System.EventHandler<int>(Overflow);
            }
            Init();
        }

        private void fillMappingArray()
        {
            MappingArray = new int[256];
            for (int index = 0; index < 255; index++)
            {
                    MappingArray[index] = index;
            }
            FillMappingArrayExceptions();
        }

        private void FillMappingArrayExceptions()
        {
            MappingArray[0x80] = 0x00;
            MappingArray[0x82] = 0x02;
            MappingArray[0x83] = 0x03;
            MappingArray[0x84] = 0x04;
            MappingArray[0x8A] = 0x0A;
            MappingArray[0x8B] = 0x0B;
        }

        private RegisterByte readINDFReg()
        {
            var address = getFSRReg();
            if (address == 0)
                return RegisterList[0];
            else
                return getRegister(address);
        }

        private int getFSRReg()
        {
            return RegisterList[0x04].Value;
        }

        public RegisterByte[] getRegisterList {
            get { return RegisterList; }
            set { this.RegisterList = value;}
        }

        /// <summary>
        /// Gibt ein Register anhand des Index zurück
        /// </summary>
        /// <param name="index">Gibt die Speicherzelle des Byte an</param>
        /// <returns>gewählte Speicherzelle</returns>
        public RegisterByte getRegister(int index)
        {
            if (index == 0)
                return readINDFReg();
            if (isRegisterBankSelectBitSet())
                index = MappingArray[index + 0x80];
            else
                index = MappingArray[index];
            return this.RegisterList[index];
        }

        public RegisterByte getStatusRegisterContent()
        {
            return RegisterList[3];
        }


        internal void SetCarryBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value | 0x01);
        }

        internal void ResetCarryBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value & 0xFE);
        }

        public bool getCarryBit()
        {
            if ( (RegisterList[3].Value & 0x01) == 0x01)
                return true;
            else
                return false;
        }

        internal void SetDigitCarryBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value | 0x02);
        }

        internal void ResetDigitCarryBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value & 0xFD);
        }

        public bool getDigitCarryBit()
        {
            if ((RegisterList[3].Value & 0x02) == 0x02)
                return true;
            else
                return false;
        }

        internal void SetZeroBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value | 0x04);
        }

        internal void ResetZeroBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value & 0xFB);
        }

        public bool getZeroBit()
        {
            if ((RegisterList[3].Value & 0x02) == 0x04)
                return true;
            else
                return false;
        }

        public void SetPowerDownBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value | 0x08);
        }

        public void ResetPowerDownBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value & 0xF7);
        }

        public void SetTimeOutBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value | 0x10);
        }

        public void ResetTimeOutBit()
        {
            RegisterList[3].Value = (byte) (RegisterList[3].Value & 0xEF);
        }

        public void Init()
        {
            this.getRegister(0x02).Value = 0x00; 
            this.getRegister(0x03).Value = 0x18;
            this.getRegister(0x0A).Value = 0x00;
            this.getRegister(0x0B).Value = 0x00;
            this.getRegister(0x81).Value = 0xFF;
            this.getRegister(0x85).Value = 0x1F;
            this.getRegister(0x86).Value = 0xFF;
            this.getRegister(0x88).Value = 0x00;
        }

        public void ClearRegister()
        {
            foreach (var registerByte in RegisterList)
            {
                registerByte.Value = 0;
            }
        }

        public bool isRegisterBankSelectBitSet()
        {
            if ((RegisterList[3].Value & 0x20) == 0x20)
                return true;
            else
                return false;
        }

        public void RegisterContentChanged(object sender, RegisterByte registerToChange)
        {
            RegisterList.Where(item => item.Index == registerToChange.Index).Select(item => item.Value = registerToChange.Value);
        }
        
      
        /// <summary>
        /// Setzt ein bestimmtes Bit in einem Byte.
        /// </summary>
        /// <param name="b">Byte, welches bearbeitet werden soll.</param>
        /// <param name="BitNumber">Das zu setzende Bit (0 bis 7).</param>
        /// <returns>Ergebnis - Byte</returns>
        public static byte SetBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        /// <summary>
        /// Löscht ein bestimmtes Bit in einem Byte.
        /// </summary>
        /// <param name="b">Byte, welches bearbeitet werden soll.</param>
        /// <param name="BitNumber">Das zu löschende Bit (0 bis 7).</param>
        /// <returns>Ergebnis - Byte</returns>
        public static byte ClearBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b & (byte)(~BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

    }

    public enum TimerStatus
    {
        TIMER,
        COUNTER
    }
 
}
