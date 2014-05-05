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
        //Operands: 0 £ k £ 255
        //Operation: k ® (W);
        //TOS ® PC
        //Status Affected: None
        //Description: The W register is loaded with the
        //eight-bit literal 'k'. The program
        //counter is loaded from the top of
        //the stack (the return address).
        //This is a two-cycle instruction.
        protected override void execute()
        {
            //TODO
        }

        public RETLW(int k)
        {
            this.k = k;

            execute();
        }
    }
}
