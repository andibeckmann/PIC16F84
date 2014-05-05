using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class SUBLW : BaseOperation
    {
        //SUBLW Subtract W from Literal
        //Syntax: [ label ] SUBLW k
        //Operands: 0 < k < 255
        //Operation: k - (W) -> (W)
        //Status Affected: C, DC, Z
        //Description: The W register is subtracted (2’s
        //complement method) from the
        //eight-bit literal 'k'. The result is
        //placed in the W register.

        protected override void execute(WorkingRegister W)
        {
            W.Value.GetRegisterMap().SetCarryBit();
            W.Value.GetRegisterMap().SetDigitCarryBit();
            W.Value.GetRegisterMap().SetZeroBit();

            W.Value.Value = (sbyte) ((int)k - (int)W.Value.Value);
        }

        public SUBLW(sbyte k, WorkingRegister W)
        {
            this.k = k;

            execute(W);
        }
    }
}
