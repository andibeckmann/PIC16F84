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
        public event EventHandler<int> StackChanged;
        public int Level { get; set; }

        public ProgramMemoryAddress()
        {
            value = 0;
            this.Level = 0;
        }

        public ProgramMemoryAddress(int address)
        {
            value = 0;
            this.Level = 0;
        }

        public ProgramMemoryAddress(int address, int level)
        {
            value = address;
            this.Level = level;
        }

        public int Value
        {
            get { return value; }
            set { this.value = value;
                if (this.StackChanged != null)
                {
                    this.StackChanged(this, Level);
                }
            }
        }
    }
}
