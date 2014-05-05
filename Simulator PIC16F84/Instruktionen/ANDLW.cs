using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class ANDLW : BaseOperation
    {
        //ANDLW             AND Literal with W
        //--------------------------------------
        //Syntax:           [label] ANDLW k
        //Operands:         0 <= k <= 255
        //Operation:        (W) .AND. (k) -> (W)
        //Status Affected:  Z
        //Description:      The contents of W register are
        //                  AND’ed with the eight-bit literal
        //                  'k'. The result is placed in the W
        //                  register.

        public ANDLW(int k, WorkingRegister W)
        {
            this.f = f;
            this.b = b;

            execute(W);
        }

        protected override void execute(WorkingRegister W)
        {
            W.Value.Value = (sbyte) (W.Value.Value & k);
        }
    }
}
