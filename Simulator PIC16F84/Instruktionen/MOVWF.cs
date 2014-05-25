using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// MOVWF             Move W to f
    /// Syntax:           [label] MOVWF f
    /// Operands:         0 &lt;= f &lt;= 127
    /// Operation:        (W) -> (f)
    /// Status Affected:  None
    /// Description:      Move data from W register to
    ///                   register 'f'.
    /// </summary>
    public class MOVWF : BaseOperation
    {
        

        private byte content;

        public MOVWF(int f, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            content = W.Value;
            Reg.getRegister(f).Value = content;
        }
    }
}
