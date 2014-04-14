using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class ProgramMemoryByte
    {
        private int value;

        public ProgramMemoryByte()
        {
            value = 0;
        }

        public int Value {
            get { return value; }
            set { this.value = value; }
        }
    }
}
