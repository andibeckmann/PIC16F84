using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class MOVF : BaseOperation
    {
        //MOVF              Move f,d
        //--------------------------------------
        //Syntax:           [label] IMOVF f,d
        //Operands:         0 <= f <= 127
        //                  d e [0,1]
        //Operation:        (f) -> (destination)
        //Status Affected:  Z
        //Description:      The contents of register f are
        //                  moved to a destination dependant
        //                  upon the status of d. If d = 0, destination
        //                  is W register. If d = 1, the
        //                  destination is file register f itself.
        //                  d = 1 is useful to test a file register,
        //                  since status flag Z is affected.

        private int content;
        private RegisterFileMap Register;

        public MOVF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.d = d;
            this.W = W;
            Register = W.Value.GetRegisterMap();

            execute();
        }

        protected override void execute()
        {
            content = Register.RegisterList[f].Value;

            if (d)
                Register.RegisterList[f].Value = content;
            else
                W.Value.Value = content;

            if (content == 0)
                Register.SetZeroBit();
        }
    }
}
