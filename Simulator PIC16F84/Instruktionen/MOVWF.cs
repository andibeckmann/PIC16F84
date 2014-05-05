using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class MOVWF : BaseOperation
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

            execute(W);
        }

        protected override void execute(WorkingRegister W)
        {
            content = W.Value.Value;
            W.Value.GetRegisterMap().getRegisterList[f].Value = content;
        }
    }
}
