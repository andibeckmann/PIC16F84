using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class XORWF : BaseOperation
    {
        //XORWF Exclusive OR W with f
        //Syntax: [label] XORWF f,d
        //Operands: 0 < f < 127
        //d e [0,1]
        //Operation: (W) .XOR. (f) -> (destination)
        //Status Affected: Z
        //Description: Exclusive OR the contents of the
        //W register with register 'f'. If 'd' is
        //0, the result is stored in the W
        //register. If 'd' is 1, the result is
        //stored back in register 'f'.
        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            Reg.SetZeroBit();

            var result = W.Value.Value ^ Reg.getRegister(f).Value;

            if(d)
            {
                Reg.getRegister(f).Value = (byte) result;
            }
            else
            {
                W.Value.Value = (byte) result;
            }
        }

        public XORWF(int f, bool d, WorkingRegister W, RegisterFileMap Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }
    }
}
