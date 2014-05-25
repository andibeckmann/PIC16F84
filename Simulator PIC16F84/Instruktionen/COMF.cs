using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// COMF Complement f
    /// Syntax: [ label ] COMF f,d
    /// Operands: 0 ≤ f ≤ 127
    /// d ∈ [0,1]
    /// Operation: ~(f) → (destination)
    /// Status Affected: Z
    /// Description: The contents of register ’f’ are 
    /// complemented. If ’d’ is 0, the 
    /// result is stored in W. If ’d’ is 1, the 
    /// result is stored back in register ’f’
    /// </summary>
    public class COMF : BaseOperation
    {
        public COMF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);

        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            Reg.SetZeroBit();

            var reverseResult = ~(Reg.getRegister(f).Value);

            if (d)
            {
                Reg.getRegister(f).Value = (byte)reverseResult;
            }
            else
            {
                W.Value = (byte)reverseResult;
            } 
        }
    }
}
