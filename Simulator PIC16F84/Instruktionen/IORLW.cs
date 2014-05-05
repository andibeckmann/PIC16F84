using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class IORLW : BaseOperation
    {
        //IORLW Inclusive OR Literal with W
        //Syntax: [ label ] IORLW k
        //Operands: 0 < k < 255
        //Operation: (W) .OR. k -> (W)
        //Status Affected: Z
        //Description: The contents of the W register are
        //OR’ed with the eight-bit literal 'k'.
        //The result is placed in the W
        //register.

        public IORLW(int k, WorkingRegister W)
        {
            this.k = k;
            execute();
        }

        protected override void execute()
        {
            W.Value.Value |= k;
        }

        
    }
}
