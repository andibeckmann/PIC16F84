using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class CLRF : BaseOperation
    {
        //CLRF              Clear f
        //--------------------------------------
        //Syntax:           [label] CLRF f
        //Operands:         0 <= f <= 127
        //Operation:        00h -> (f)
        //                  1 -> Z
        //Status Affected:  Z
        //Description:      The contents of register 'f' are
        //                  cleared and the Z bit is set.

        private RegisterFileMap Register;

        public CLRF(int f, WorkingRegister W)
        {
            this.f = f;
            this.W = W;
            this.Register = W.Value.GetRegisterMap();
            execute();
        }

        protected override void execute()
        {
            this.Register.RegisterList[f].ClearRegister();
            this.Register.SetZeroBit();
        }
    }
}
