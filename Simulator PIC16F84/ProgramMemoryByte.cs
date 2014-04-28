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

        public void DecodeInstruction(WorkingRegister W)
        {
            int k;
            int f;
            int b;
            bool d;

            //ADDLW Instruktion
            if ((value & (int)0x3E00) == (int)0x3E00)
            {
                k = value & (int)0xFF;
                ADDLW AddlwOperation = new ADDLW( k, W );
            }

            //BCF Instruktion
            if ((value & (int)0x1000) == (int)0x1000)
            {
                f = value & (int)0x7F;
                b = value & (int)0x380;
                BCF AddlwOperation = new BCF(f, b, W);
            }

            //ADDWF Instruktion
            if ((value & (int)0x0700) == (int)0x0700)
            {
                f = value & (int)0x7F;
                if ((value & 0x80) == 0x80)
                    d = true;
                else
                    d = false;
                ADDWF AddlwOperation = new ADDWF(f, d, W);
            }


        }

    }
}
