using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class RETURN : BaseOperation
    {
        //RETURN Return from Subroutine
        //Syntax: [ label ] RETURN
        //Operands: None
        //Operation: TOS ® PC
        //Status Affected: None
        //Description: Return from subroutine. The stack
        //is POPed and the top of the stack
        //(TOS) is loaded into the program
        //counter. This is a two-cycle
        //instruction.

        public RETURN(ProgramCounter PC, Stack Stack, RegisterFileMap Reg) : base(Reg)
        {
            this.Stack = Stack;
            Reg.IncrementTimer();

            execute(PC);
        }

        protected void execute(ProgramCounter PC)
        {
            PC.Counter.Value = Stack.PullFromStack().Value;
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
