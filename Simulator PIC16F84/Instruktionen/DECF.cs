using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// COMF Decrement f
    /// Syntax: [ label ] DECF f,d
    /// Operands: 0 ≤ f ≤ 127
    /// d ∈ [0,1]
    /// Operation: (f) -1  → (destination)
    /// Status Affected: Z
    /// Description: Decrement register 'f'.  
    /// If ’d’ is 0, the 
    /// result is stored in W. If ’d’ is 1, the 
    /// result is stored back in register ’f’
    /// </summary>
    class DECF : BaseOperation
    {
        public DECF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
           Reg.SetZeroBit();

            var result = Reg.getRegister(f).Value - 1;

            if (d)
            {
               Reg.getRegister(f).Value = (byte) result;
            }
            else
            {
                W.Value = (byte) result;
            }
        }
    }
}
