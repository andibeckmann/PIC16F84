using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class RETFIE : BaseOperation
    {
        //RETFIE Return from Interrupt
        //Syntax: [ label ] RETFIE
        //Operands: None
        //Operation: TOS -> PC,
        //1 -> GIE
        //Status Affected: None
        public RETFIE(ProgramCounter PC, Stack stack)
        {
            execute(PC, stack);
        }

        protected void execute(ProgramCounter PC, Stack stack)
        {
            PC.Counter = stack.PullFromStack().Value;
            //TODO set GIE = 1;
        }

        protected override void execute(WorkingRegister W)
        {
            throw new NotImplementedException();
        }
    }
}
