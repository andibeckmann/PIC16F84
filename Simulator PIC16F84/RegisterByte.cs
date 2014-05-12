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
        private int index;

        public event EventHandler<int> RegisterChanged;

        public RegisterByte(RegisterFileMap registerFileMap, int index)
        {
            this.registerFileMap = registerFileMap;
            value = 0;
            this.index = index;
        }

        public sbyte Value {
            get { return value; }
            set
            {
                if (value != this.value)
                {
                    this.value = value;
                    if (this.RegisterChanged != null)
                        this.RegisterChanged(this, index);
                }
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

        public sbyte FormComplement()
        {
            return ((sbyte) ((int) value ^ 0xff));
        }
    }

}
