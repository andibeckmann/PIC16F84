using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class CLRF : BaseOperation
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


        public CLRF(int f, RegisterFileMap Reg)
        {
            this.f = f;
            execute(Reg);
        }

        protected void execute(RegisterFileMap Reg)
        {
            Reg.getRegisterList[f].ClearRegister();
            Reg.SetZeroBit();
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
