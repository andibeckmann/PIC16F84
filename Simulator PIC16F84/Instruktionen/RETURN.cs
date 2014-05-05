using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class RETURN : BaseOperation
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

        public RETURN(ProgramCounter PC, Stack Stack)
        {
            this.Stack = Stack;

            execute(PC);
        }

        protected override void execute(ProgramCounter PC)
        {
            PC.Counter = Stack.PullFromStack().Value;
        }
    }
}
