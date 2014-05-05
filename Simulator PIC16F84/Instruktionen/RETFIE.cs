using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class RETFIE : BaseOperation
    {
        //RETFIE Return from Interrupt
        //Syntax: [ label ] RETFIE
        //Operands: None
        //Operation: TOS -> PC,
        //1 -> GIE
        //Status Affected: None
        public RETFIE()
        {

        }

        protected override void execute()
        {
            throw new NotImplementedException();
        }
    }
}
