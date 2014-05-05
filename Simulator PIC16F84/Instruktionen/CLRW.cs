using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class CLRW : BaseOperation
    {
        //CLRW              Clear W
        //--------------------------------------
        //Syntax:           [label] CLRW
        //Operands:         None
        //Operation:        00h -> (W)
        //                  1 -> Z
        //Status Affected:  Z
        //Description:      W register is cleared. Zero bit (Z)
        //                  is set.

        public CLRW(WorkingRegister W)
        {
            this.W = W;

            execute();
        }

        protected override void execute()
        {
            this.W.ClearWorkingRegister();
            this.W.Value.GetRegisterMap().SetZeroBit();
        }
    }
}

