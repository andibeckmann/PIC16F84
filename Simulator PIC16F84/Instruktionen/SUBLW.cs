using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// SUBLW Subtract W from Literal
    /// Syntax: [ label ] SUBLW k
    /// Operands: 0 &lt; k &lt; 255
    /// Operation: k - (W) -> (W)
    /// Status Affected: C, DC, Z
    /// Description: The W register is subtracted (2’s
    /// complement method) from the
    /// eight-bit literal 'k'. The result is
    /// placed in the W register.
    /// </summary>
    class SUBLW : BaseOperation
    {
        

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            Reg.SetCarryBit();
            Reg.SetDigitCarryBit();
            Reg.SetZeroBit();

            W.Value = (byte) ((int)k - (int)W.Value);
        }

        public SUBLW(byte k, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;

            execute(W, Reg);
        }
    }
}
