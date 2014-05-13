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

        public RegisterFileMap()
        {
            fillMappingArray();
            RegisterList = new RegisterByte[256];
            for (int var = 0; var < RegisterList.Length; var++ )
            {
                RegisterList[var] = new RegisterByte(var);
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
        
      
    }
 
}
