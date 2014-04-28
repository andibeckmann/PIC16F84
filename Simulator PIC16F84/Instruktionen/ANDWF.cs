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

        private int result;
        private RegisterFileMap Register;
        private new RegisterByte f;
        private new RegisterByte W;

        public ANDWF(int file, bool d, WorkingRegister WReg)
        {
            W = WReg.Value;
            Register = W.GetRegisterMap();
            f = Register.RegisterList[file];

            result = W.Value & f.Value;

            //Zero-Bit Logik
            if (result == 0)
                Register.SetZeroBit();
            else
                Register.ResetZeroBit();

            //Unterscheidung Working Reg oder File Reg
            if (d)
                f.Value = result;
            else
                W.Value = result;
        }
    }
}
