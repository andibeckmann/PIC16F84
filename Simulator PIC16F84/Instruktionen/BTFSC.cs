using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// BTFSC             Bit Test, Skip if Clear
    /// Syntax:           [label] BTFSC f,b
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   0 &lt;= b &lt;= 7
    /// Operation:        skip if (f&lt;b>) = 0
    /// Status Affected:  None
    /// Description:      If bit 'bit' in register 'f' is 1, the next
    ///                   instruction is discarded, and a NOP
    ///                   is executed instead, making this a
    ///                   2TCY instruction
    /// </summary>
    public class BTFSC : BaseOperation
    {
        

        public BTFSC(int file, int b, ProgramCounter PC, RegisterFileMap Reg) : base(Reg)
        {
            this.f = file;
            this.b = b;

            execute(Reg, PC);
        }

        protected void execute(RegisterFileMap Reg, ProgramCounter PC)
        {
            if (!IsBitSet(Reg.getRegister(f).Value, b))
            {
                PC.Counter.Address++;
                NOP Operation = new NOP(Reg);
            }
        }


        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
