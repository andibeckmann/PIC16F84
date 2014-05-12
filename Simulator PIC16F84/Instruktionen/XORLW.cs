using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class XORLW : BaseOperation
    {
        //XORLW             Exclusive OR Literal with W
        //--------------------------------------
        //Syntax:           [label]  XORLW k
        //Operands:         0 <= k <= 255
        //Operation:        (W) .XOR.k -> (W)
        //Status Affected:  Z
        //Description:      The contents of the W register
        //                  are XOR’ed with the eight-bit literal
        //                  'k'. The result is placed in
        //                  the W register.

        private int result;

        public XORLW(byte k, WorkingRegister W)
        {
            this.k = k;
            execute(W);
        }

        protected override void execute(WorkingRegister W)
        {
            result = W.Value.Value ^ k;
            W.Value.Value = (byte) result;

            if (result == 0)
                W.Value.GetRegisterMap().SetZeroBit();
            else
                W.Value.GetRegisterMap().ResetZeroBit();
        }
    }
}
