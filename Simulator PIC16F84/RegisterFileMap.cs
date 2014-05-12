using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterFileMap
    {
        public RegisterByte[] RegisterList;

        public RegisterFileMap()
        {
            RegisterList = new RegisterByte[256];
            for (int var = 0; var < RegisterList.Length; var++ )
            {
                RegisterList[var] = new RegisterByte(ref this,var);
            }
        }

        public RegisterByte[] getRegisterList {
            get { return RegisterList; }
            set { this.RegisterList = value;}
        }

        //er.... ???
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

        
        
      
    }
 
}
