using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class GOTO : BaseOperation
    {
        //GOTO              Unconditional Branch
        //--------------------------------------
        //Syntax:           [label] GOTO k
        //Operands:         0 <= k <= 2047
        //Operation:        k -> PC<10:0>
        //                  PCLATH<4:3> -> PC<12:11>
        //Status Affected:  None
        //Description:      GOTO is an unconditional branch.
        //                  The eleven-bit immediate value is
        //                  loaded into PC bits <10:0>. The
        //                  upper bits of PC are loaded from
        //                  PCLATH<4:3>. GOTO i a two-
        //                  cycle instruction.

        public GOTO(sbyte k, ProgramCounter PC)
        {
            this.k = k;

            execute(PC);
        }

        protected void execute(ProgramCounter PC)
        {
            PC.Counter = k;

            //TODO: PCLATH - siehe Beschreibung, wurde hier noch komplett ignoriert
        }

        protected override void execute(WorkingRegister W)
        {
            throw new NotImplementedException();
        }
    }
}
