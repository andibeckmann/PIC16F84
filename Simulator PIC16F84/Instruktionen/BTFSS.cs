using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// BTFSS Bit Test f, Skip if Set
    /// Syntax: [label] BTFSS f,b
    /// Operands: 0 ≤ f ≤ 127
    /// 0 ≤ b ≤ 7
    /// Operation: skip is(f&lt;b>) = 1
    /// Status Affected: None
    /// Description: If bit 'b' in register 'f' is '0', the next
    /// instruction is executed.
    /// If Bit 'b' is '1', then the next instruction is discarded 
    /// and a NOP is executed instead, making this a 2TCY instruction.
    /// </summary>
    public class BTFSS : BaseOperation
    {
        

        public BTFSS(int f, int b, ProgramCounter PC, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.b = b;
            this.W = W;

            execute(Reg, PC); 
        }

        protected void execute(RegisterFileMap Reg, ProgramCounter PC)
        {
            if( IsBitSet(Reg.getRegister(f).Value, b))
            {
                PC.Counter.Value++;
                new NOP(Reg);
            }
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
