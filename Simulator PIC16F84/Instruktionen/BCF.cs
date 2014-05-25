using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// BCF Bit Clear f
    /// Syntax: [label] BCF f,b
    /// Operands: 0 ≤ f ≤ 127
    /// 0 ≤ b ≤ 7
    /// Operation: 0 → (f&lt;b>)
    /// Status Affected: None
    /// Description: Bit 'b' in register 'f' is cleared
    /// </summary>
    public class BCF : BaseOperation
    {
        

        public BCF(int f, int b, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.b = b;

            execute(Reg);
        }

        protected void execute(RegisterFileMap Reg)
        {
            Reg.getRegister(f).Value = TurnBitOff(Reg.getRegister(f).Value, b);
        }

        

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }

    }
}
