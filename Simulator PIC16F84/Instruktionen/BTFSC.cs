using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class BTFSC : BaseOperation
    {
        //BTFSC             Bit Test, Skip if Clear
        //--------------------------------------
        //Syntax:           [label] BTFSC f,b
        //Operands:         0 <= f <= 127
        //                  0 <= b <= 7
        //Operation:        skip if (f<b>) = 0
        //Status Affected:  None
        //Description:      If bit 'bit' in register 'f' is 1, the next
        //                  instruction is discarded, and a NOP
        //                  is executed instead, making this a
        //                  2TCY instruction

        public BTFSC(int file, int b, ProgramCounter PC, RegisterFileMap Reg)
        {
            this.f = file;
            this.b = b;

            execute(Reg, PC);
        }

        protected void execute(RegisterFileMap Reg, ProgramCounter PC)
        {
            if (IsBitSet(Reg.getRegister(f).Value, b))
            {
                PC.Counter.Value++;
                NOP Operation = new NOP();
            }
        }


        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
