using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// ANDLW             AND Literal with W
    /// Syntax:           [label] ANDLW k
    /// Operands:         0 &lt;= k &lt;= 255
    /// Operation:        (W) .AND. (k) -> (W)
    /// Status Affected:  Z
    /// Description:      The contents of W register are
    ///                   AND’ed with the eight-bit literal
    ///                   'k'. The result is placed in the W
    ///                   register.
    /// </summary>
    class ANDLW : BaseOperation
    {
        

        public ANDLW(int k, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.b = b;

            execute(W);
        }

        private void execute(WorkingRegister W)
        {
            W.Value = (byte) (W.Value & k);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
