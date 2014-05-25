using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// XORWF Exclusive OR W with f
    /// Syntax: [label] XORWF f,d
    /// / Operands: 0 &lt; f &lt; 127
    /// d e [0,1]
    /// Operation: (W) .XOR. (f) -> (destination)
    /// Status Affected: Z
    /// Description: Exclusive OR the contents of the
    /// W register with register 'f'. If 'd' is
    /// 0, the result is stored in the W
    /// register. If 'd' is 1, the result is
    /// stored back in register 'f'.
    /// </summary>
    class XORWF : BaseOperation
    {
        
        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            Reg.SetZeroBit();

            var result = W.Value ^ Reg.getRegister(f).Value;

            if(d)
            {
                Reg.getRegister(f).Value = (byte) result;
            }
            else
            {
                W.Value = (byte) result;
            }
        }

        public XORWF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }
    }
}
