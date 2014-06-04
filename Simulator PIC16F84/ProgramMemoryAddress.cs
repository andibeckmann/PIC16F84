using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class ProgramMemoryAddress
    {
        private int address;

        public ProgramMemoryAddress()
        {
            address = 0;
        }

        public ProgramMemoryAddress(int address)
        {
            this.address = address;
        }

        public void setBit(int pos)
        {
            address = (byte)((int)address | (0x01 << pos));
        }

        public bool isBitSet(int pos)
        {
            return (((address >> pos) & 0x1) == 0x1);
        }

        public int Address
        {
            get { return address; }
            set
            {
                if ( value < 0x3FFF && value >= 0 )
                    this.address = value;
                else
                   throw new ArgumentOutOfRangeException();
            }
        }
    }
}
