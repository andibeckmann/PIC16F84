using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterByte
    {
        private int value;
        private RegisterFileMap registerFileMap;

        public RegisterByte(RegisterFileMap registerFileMap)
        {
            this.registerFileMap = registerFileMap;
            value = 0;
        }

        public int Value {
            get { return value; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();
                if (value > 0xFF)
                    value = value % 0x100;
                this.value = value;
            }
        }

        public RegisterFileMap GetRegisterMap()
        {
            return registerFileMap;
        }


        public void ClearRegister()
        {
            value = 0;
        }

        public int DecrementRegister()
        {
            return value--;
        }
    }

}
