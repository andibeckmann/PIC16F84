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
        public SUBLW(byte k, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            var result = k + ~W.Value + 1;

            /// Zero-Bit Logik
            if (result == 0 || result == 256)
                Reg.SetZeroBit();
            else
                Reg.clearZeroBit();

            /// Carry-Bit Logik
            if (result < k && result >= 0)
                Reg.SetCarryBit();
            else
                Reg.ResetCarryBit();

            /// Digit Carry Logik
            if ((W.Value & 0x0F) + (k & 0x0F) > 0x0f)
                Reg.SetDigitCarryBit();
            else
                Reg.ResetDigitCarryBit();

            W.Value = (byte)result;
        }
    }
}
