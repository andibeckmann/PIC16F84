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
        protected override void execute(WorkingRegister W)
        {
            W.Value.GetRegisterMap().SetZeroBit();

            var result = W.Value.Value ^ W.Value.GetRegisterMap().getRegisterList[f].Value;

            if(d)
            {
                W.Value.GetRegisterMap().getRegisterList[f].Value = (sbyte) result;
            }
            else
            {
                W.Value.Value = (sbyte) result;
            }
        }

        public XORWF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.d = d;

            execute(W);
        }
    }
}
