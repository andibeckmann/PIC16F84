using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class ProgramCounter
    {
        private ProgramMemoryAddress counter;


        public ProgramCounter()
        {
            counter = new ProgramMemoryAddress(0);
        }

        public int Counter
        {
            get { return counter.Value; }
            set { this.counter.Value = value; }
        }
    }
}
