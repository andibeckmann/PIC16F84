using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class CALL : BaseOperation
    {
        //CALL              Call Subroutine
        //--------------------------------------
        //Syntax:           [label] CALL k
        //Operands:         0 <= k <= 2047
        //Operation:        (PC) + 1 -> TOS,
        //                  k -> PC<10:0>,
        //                  (PCLATH<4:3>) -> PC<12:11>
        //Status Affected:  None
        //Description:      Call Subroutine. First, return
        //                  address (PC+1) is pushed onto
        //                  the stack. The eleven-bit immedi-
        //                  ate address is loaded into PC bits
        //                  <10:0>. the upper bits of the PC
        //                  are loaded from PCLATH. CALL is
        //                  a two-cycle instruction.

        public CALL( int k )
        {
            this.k = k;

            execute();
        }

    private void execute()
    {
 	
    }
    }
}
