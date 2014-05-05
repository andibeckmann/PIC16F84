using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class MOVWF : BaseOperation
    {
        //MOVWF             Move W to f
        //--------------------------------------
        //Syntax:           [label] MOVWF f
        //Operands:         0 <= f <= 127
        //Operation:        (W) -> (f)
        //Status Affected:  None
        //Description:      Move data from W register to
        //                  register 'f'.

        private int content;
        private RegisterFileMap Register;

        public MOVWF(int f, WorkingRegister W)
        {
            this.f = f;
            this.W = W;
            Register = W.Value.GetRegisterMap();

            execute();
        }

        protected override void execute()
        {
            content = W.Value.Value;
            Register.RegisterList[f].Value = content;
        }
    }
}
