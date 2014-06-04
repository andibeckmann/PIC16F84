using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// INCFSZ Increment f, Skip if 0
    /// Syntax: [ label ] INCFSZ f,d
    /// Operands: 0 &lt; f &lt; 127
    /// d ∈ [0,1]
    /// Operation: (f) + 1 → (destination),
    /// skip if result = 0
    /// Status Affected: None
    /// Description: The contents of register ’f’ are
    /// incremented. If ’d’ is 0, the result is
    /// placed in the W register. If ’d’ is 1,
    /// the result is placed back in
    /// register ’f’.
    /// If the result is 1, the next instruction
    /// is executed. If the result is 0,
    /// a NOP is executed instead, making
    /// it a 2TCY instruction.
    /// </summary>
    public class INCFSZ : BaseOperation
    {
        

        public INCFSZ(int f, bool d, WorkingRegister W, RegisterFileMap Reg, ProgramCounter PC) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg, PC);
        }

        protected void execute(WorkingRegister W, RegisterFileMap Reg, ProgramCounter PC)
        {
            var result = (byte) (Reg.getRegister(f).Value + 1);

            if (d)
            {
                Reg.getRegister(f).Value = result;   
            }
            else
            {
                W.Value = result;
            }
            if(result == 0)
            {
                PC.Counter.Address++;
                new NOP(Reg);
            }
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
