using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class RETLW : BaseOperation
    {
        //RETLW Return with Literal in W
        //Syntax: [ label ] RETLW k
        //Operands: 0 < k < 255
        //Operation: k -> (W);
        //TOS -> PC
        //Status Affected: None
        //Description: The W register is loaded with the
        //eight-bit literal 'k'. The program
        //counter is loaded from the top of
        //the stack (the return address).
        //This is a two-cycle instruction.
        protected void execute(WorkingRegister W, ProgramCounter PC, Stack stack)
        {
            W.Value.Value = k;

            PC.Counter.Value = stack.PullFromStack().Value;
        }

        public RETLW(byte k, WorkingRegister W, ProgramCounter PC, Stack stack, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;

            execute(W, PC, stack);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
