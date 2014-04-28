using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class ProgramMemoryAddress
    {
        private int value;

        public ProgramMemoryAddress()
        {
            value = 0;
        }

        public ProgramMemoryAddress(int address)
        {
            value = address;
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
