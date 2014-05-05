using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class ANDWF : BaseOperation
    {
        //ANDWF             AND W with f
        //--------------------------------------
        //Syntax:           [label] ANDWF f,d
        //Operands:         0 <= f <= 127
        //                  d e [0,1]
        //Operation:        (W).AND.(f) -> (destination)
        //Status Affected:  Z
        //Description:      AND the W register with register
        //                  'f'. If 'd' is 0, the result is stored in
        //                  the W register. If 'd' is 1, the result
        //                  is stored back in register 'f'.

        public ANDWF(int f, bool d, WorkingRegister W)
        {
            this.d = d;
            this.f = f;
            execute(W);
        }

        protected override void execute(WorkingRegister W)
        {
            var result = W.Value.Value & W.Value.GetRegisterMap().getRegisterList[f].Value;

            //Zero-Bit Logik
            if (result == 0)
                W.Value.GetRegisterMap().SetZeroBit();
            else
                W.Value.GetRegisterMap().ResetZeroBit();

            //Unterscheidung Working Reg oder File Reg
            if (d)
                W.Value.GetRegisterMap().RegisterList[f].Value = (sbyte)result;
            else
                W.Value.Value = (sbyte)result;
        }
    }
}
