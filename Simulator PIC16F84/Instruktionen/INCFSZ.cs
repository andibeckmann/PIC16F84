using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class INCFSZ : BaseOperation
    {
        //INCFSZ Increment f, Skip if 0
        //Syntax: [ label ] INCFSZ f,d
        //Operands: 0 < f < 127
        //d ∈ [0,1]
        //Operation: (f) + 1 → (destination),
        //skip if result = 0
        //Status Affected: None
        //Description: The contents of register ’f’ are
        //incremented. If ’d’ is 0, the result is
        //placed in the W register. If ’d’ is 1,
        //the result is placed back in
        //register ’f’.
        //If the result is 1, the next instruction
        //is executed. If the result is 0,
        //a NOP is executed instead, making
        //it a 2TCY instruction.

        public INCFSZ(int f, bool d, WorkingRegister W, ProgramCounter PC)
        {
            this.f = f;
            this.d = d;

            execute(W, PC);
        }

        protected void execute(WorkingRegister W, ProgramCounter PC)
        {
            var result = (byte) (W.Value.GetRegisterMap().getRegisterList[f].Value + 1);

            if (d)
            {
                W.Value.GetRegisterMap().getRegisterList[f].Value = result;   
            }
            else
            {
                W.Value.Value = result;
            }
            if(result == 0)
            {
                PC.Counter.Value++;
                new NOP();
            }
        }

        protected override void execute(WorkingRegister W)
        {
            throw new NotImplementedException();
        }
    }
}
