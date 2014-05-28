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
                this.value = value; 
                if (this.RegisterChanged != null)
                {
                    this.RegisterChanged(this, Index);
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

        public bool isBitSet(int pos)
        {
            return ( ( ( value >> pos ) & 0x1 ) == 0x1 );
        }

        public void clearBit(int pos)
        {
            value = (byte) ((int)value & (0xFE << pos));
        }

        public void setBit(int pos)
        {
            value = (byte)((int)value | (0x01 << pos));
        }

        public byte FormComplement()
        {
            return ((byte) ((int) value ^ 0xff));
        }

        public bool checkForFallingEdge(byte oldValue, int bitPosition)
        {
            var bytevalue = 0x01 << bitPosition;
            return (!isBitSet(bitPosition) && ((oldValue & bytevalue) == bytevalue));
        }

        public bool checkForRisingEdge(byte oldValue, int bitPosition)
        {
            var bytevalue = 0x01 << bitPosition;
            return (isBitSet(bitPosition) && ((oldValue & bytevalue) != bytevalue));
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
