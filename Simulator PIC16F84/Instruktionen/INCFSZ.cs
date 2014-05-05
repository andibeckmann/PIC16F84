using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class INCFSZ : BaseOperation
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

        public INCFSZ(int f, bool d)
        {
            this.f = f;
            this.d = d;

            execute();
        }

        protected override void execute()
        {
            
        }
    }
}
