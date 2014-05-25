using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// XORLW             Exclusive OR Literal with W
    /// Syntax:           [label]  XORLW k
    /// Operands:         0 &lt;= k &lt;= 255
    /// Operation:        (W) .XOR.k -> (W)
    /// Status Affected:  Z
    /// Description:      The contents of the W register
    ///                   are XOR’ed with the eight-bit literal
    ///                   'k'. The result is placed in
    ///                   the W register.
    /// </summary>
    class XORLW : BaseOperation
    {
        

        private int result;

        public XORLW(byte k, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;
            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            result = W.Value ^ k;
            W.Value = (byte) result;

            if (result == 0)
                Reg.SetZeroBit();
            else
                Reg.ResetZeroBit();
        }
    }
}
