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

        public void DecodeInstruction(WorkingRegister WorkingRegister)
        {
            int literal;

            if ((value & (int)0x3E00) == (int)0x3E00)
            {
                literal = value & (int)0xFF;
                Addlw AddlwOperation = new Addlw( literal, WorkingRegister );
            }

        }

    }
}
