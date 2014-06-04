using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// GOTO              Unconditional Branch
    /// Syntax:           [label] GOTO k
    /// Operands:         0 &lt;= k &lt;= 2047
    /// Operation:        k -> PC&lt;10:0>
    ///                   PCLATH&lt;4:3> -> PC&lt;12:11>
    /// Status Affected:  None
    /// Description:      GOTO is an unconditional branch.
    ///                   The eleven-bit immediate value is
    ///                   loaded into PC bits &lt;10:0>. The
    ///                   upper bits of PC are loaded from
    ///                   PCLATH&lt;4:3>. GOTO i a two-
    ///                   cycle instruction.
    /// </summary>
    public class GOTO : BaseOperation
    {

        public GOTO(byte k, ProgramCounter PC, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;
            Reg.ExecuteCycle();

            execute(PC);
        }

        protected void execute(ProgramCounter PC)
        {
            PC.Counter.Value = k - 1;

            //TODO: PCLATH - siehe Beschreibung, wurde hier noch komplett ignoriert
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
