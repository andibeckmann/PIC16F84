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
        private int[] MappingArray;

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
            for (int index = 0; index < 128; index++)
            {
                    MappingArray[index] = index;
                if (mappingCondition(index))
                    MappingArray[index + 128] = index;
                else
                    MappingArray[index + 128] = index + 128;
            }
        }

        private bool mappingCondition(int index)
        {
            if (index == 129 || index == 133 || index == 134 || index == 136 || index == 137 )
                return false;
            return true;
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
            this.getRegister(3).Value = 24;     //03h
            this.getRegister(129).Value = 255;   //81h
            this.getRegister(131).Value = 24;   //83h
            this.getRegister(133).Value = 31;   //85h
            this.getRegister(134).Value = 255;   //86h
        }

        public void ClearRegister()
        {
            foreach (var registerByte in RegisterList)
            {
                registerByte.Value = 0;
            }
        }
        
      
    }
 
}
