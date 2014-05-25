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

        public event EventHandler<int> RegisterChanged;
        public event EventHandler<int> Overflow;

        public bool[] fallingEdges = new bool[8];
        public bool[] risingEdges = new bool[8];

        public int Index { get; set; }

        public RegisterByte(int index)
        {
            Value = 0;
            this.Index = index;
        }

        public byte Value {
            get { return value; }
            set
            {
                CheckRisingAndFallingEdges(this.value, value);
                this.value = value; 
                if (this.RegisterChanged != null)
                {
                    this.RegisterChanged(this, Index);
                }
            }
        }

        private void CheckRisingAndFallingEdges(byte oldValue, byte newValue)
        {
            for (int i = 0; i < 8; i++)
            {
                fallingEdges[i] = false;
                risingEdges[i] = false;
                if(IsBitSet(oldValue, i) && !IsBitSet(newValue, i))
                {
                    fallingEdges[i] = true;
                }
                else if (!IsBitSet(oldValue, i) && IsBitSet(newValue, i))
                {
                    risingEdges[i] = true;
                }
            }
        }

        public void ClearRegister()
        {
            this.Value = 0;
        }

        public int DecrementRegister()
        {
            return --Value;
        }

        public int IncrementRegister()
        {
            if (value == 255)
            {
                if (this.Overflow != null)
                {
                    this.Overflow(this, Index);
                }
                ClearRegister();
                return Value;
            }
            return ++Value;
        }

        public bool IsBitSet(int pos)
        {
            return ( ( ( value >> pos ) & 0x1 ) == 0x1 );
        }

        public byte FormComplement()
        {
            return ((byte) ((int) value ^ 0xff));
        }

        /// <summary>
        /// Check if Bit is set in Byte
        /// </summary>
        /// <param name="byteValue">Byte</param>
        /// <param name="bit">Bit</param>
        /// <returns>result</returns>
        private bool IsBitSet(int byteValue, int bit)
        {
            return (byteValue & (1 << bit)) != 0;
        }

    }

}
