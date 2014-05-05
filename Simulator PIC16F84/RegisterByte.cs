using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterByte
    {
        private sbyte value;
        private RegisterFileMap registerFileMap;

        public RegisterByte(RegisterFileMap registerFileMap)
        {
            this.registerFileMap = registerFileMap;
            value = 0;
        }

        public sbyte Value {
            get { return value; }
            set
            {
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
            return value - 1;
        }

        public int IncrementRegister()
        {
            return value + 1;
        }

        public bool IsBitSet(int pos)
        {
            return ( ( ( value >> pos ) & 0x1 ) == 0x1 );
        }
    }

}
