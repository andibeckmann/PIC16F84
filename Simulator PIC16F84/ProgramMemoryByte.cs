using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simulator_PIC16F84.Instruktionen;

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
            int k;
            int f;
            int b;

            if ((value & (int)0x3E00) == (int)0x3E00)
            {
                k = value & (int)0xFF;
                ADDLW AddlwOperation = new ADDLW( k, WorkingRegister );
            }
            if ((value & (int)0x1000) == (int)0x1000)
            {
                f = value & (int)0x7F;
                b = value & (int)0x380;
                BCF AddlwOperation = new BCF(f, b);
            }

        }

    }
}
