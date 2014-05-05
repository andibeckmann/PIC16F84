using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class INCF : BaseOperation
    {
        //INCF              Increment f
        //--------------------------------------
        //Syntax:           [label] INCF f,d
        //Operands:         0 <= f <= 127
        //                  d e [0,1]
        //Operation:        (f) + 1 -> (destination)
        //Status Affected:  Z
        //Description:      The contents of register 'f' are
        //                  incremented. If 'd' is 0, the result
        //                  is placed in the W register. If 'd' is
        //                  1, the result is placed back in register 'f'.

        private int result;
        private RegisterFileMap Register;

        public INCF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.d = d;
            this.W = W;
            Register = W.Value.GetRegisterMap();

            execute();
        }

        protected override void execute()
        {
            result = Register.RegisterList[f].IncrementRegister();

            if (d)
                Register.RegisterList[f].Value = result;
            else
                W.Value.Value = result;
            if( result == 0 )
                Register.SetZeroBit();
        }
    }
}
