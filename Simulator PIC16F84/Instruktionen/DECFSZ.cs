using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class DECFSZ : BaseOperation
    {
        //DECFSZ            Decrement f, Skip if 0
        //--------------------------------------
        //Syntax:           [label] DECFSZ f,d
        //Operands:         0 <= f <= 127
        //                  d e [0,1]
        //Operation:        (f) - 1 -> (destination)
        //                  skip if result = 0
        //Status Affected:  None
        //Description:      The contents of register 'f' are
        //                  decremented. If 'd' is 0, the result
        //                  is placed in the W register. If 'd' is
        //                  1, the result is placed back in
        //                  register 'f'.
        //                  If the result is 1, the next instruc-
        //                  tion is executed. If the result is 0,
        //                  then a NOP is executed instead,
        //                  making it a 2TCY instruction.

        private RegisterFileMap Register;
        private int result;

        public DECFSZ(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.W = W;
            this.d = d;
            this.Register = W.Value.GetRegisterMap();
            execute();
        }

        protected override void execute()
        {
            result = Register.RegisterList[f].DecrementRegister();
            if (d)
                Register.RegisterList[f].Value = result;
            else
                W.Value.Value = result;

            if (result == 0)
            {
                PC.Counter++;
                NOP Operation = new NOP();
            }
         }



    }
}
