using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterByte
    {
        private byte value;
        private int index;

        public event EventHandler<int> RegisterChanged;
        public event EventHandler<int> Overflow;

        public int Index { get; set; }

        public RegisterByte(int index)
        {
            value = 0;
            this.index = index;
        }

        public byte Value {
            get { return value; }
            set
            {
                if(value > 255)
                {
                    //TODO Andi bitte...this.value = ((int) value) - 256; 
                    if (this.Overflow != null)
                    {
                        this.Overflow(this, index);
                    }
                }
                else
                {
                    this.value = value;
                }  
                if (this.RegisterChanged != null)
                {
                    this.RegisterChanged(this, index);
                }
            }
        }

        public void ClearRegister()
        {
            this.Value = 0;
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

        public byte FormComplement()
        {
            return ((byte) ((int) value ^ 0xff));
        }

    }

}
