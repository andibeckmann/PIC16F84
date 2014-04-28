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

            //ADDWF Instruktion
            if ((value & (int)0x0700) == (int)0x0700)
            {
                f = value & (int)0x7F;
                if ((value & 0x80) == 0x80)
                    d = true;
                else
                    d = false;
                ADDWF Operation = new ADDWF(f, d, W);
            }

            //BCF Instruktion
            if ((value & (int)0x1000) == (int)0x1000)
            {
                f = value & (int)0x7F;
                b = value & (int)0x380;
                BCF Operation = new BCF(f, b, W);
            }

            //BSF Instruktion
            if ((value & (int)0x1400) == (int)0x1400)
            {
                f = value & (int)0x7F;
                b = value & (int)0x380;
                BSF Operation = new BSF(f, b, W);
            }

            ////BTFSC Instruktion
            //if ((value & (int)0x1800) == (int)0x1800)
            //{
            //    f = value & (int)0x7F;
            //    b = value & (int)0x380;
            //    BTFSC Operation = new BTFSC(f, b, W);
            //}

            //BTFSS Instruktion
            if ((value & (int)0x1C00) == (int)0x1C00)
            {
                f = value & (int)0x7F;
                b = value & (int)0x380;
                BTFSS Operation = new BTFSS(f, b, W);
            }

            //ADDLW Instruktion
            if ((value & (int)0x3E00) == (int)0x3E00)
            {
                k = value & (int)0xFF;
                ADDLW Operation = new ADDLW(k, W);
            }
        }

    }
}
