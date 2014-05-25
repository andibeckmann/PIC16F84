using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// SWAPF Swap Nibbles in f
    /// Syntax: [ label ] SWAPF f,d
    /// Operands: 0 &lt; f &lt; 127
    /// d e [0,1]
    /// Operation: (f&lt;3:0>) -> (destination&lt;7:4>),
    /// (f&lt;7:4>) -> (destination&lt;3:0>)
    /// Status Affected: None
    /// Description: The upper and lower nibbles of
    /// register 'f' are exchanged. If 'd' is
    /// 0, the result is placed in W register.
    /// If 'd' is 1, the result is placed in
    /// register 'f'.
    /// </summary>
    class SWAPF : BaseOperation
    {
        
        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            var fileRegister = Reg.getRegister(f).Value;
            var result = ((fileRegister >> 4) & 0x0f) | ((fileRegister << 4) & 0xf0);

            if(d)
            {
                Reg.getRegister(f).Value = (byte) result;
            }
            else
            {
                W.Value = (byte) result;
            }
        }

        public SWAPF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }
    }
}
