﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class SUBWF : BaseOperation
    {
        //SUBWF Subtract W from f
        //Syntax: [ label ] SUBWF f,d
        //Operands: 0 < f < 127
        //d e [0,1]
        //Operation: (f) - (W) -> (destination)
        //Status Affected: C, DC, Z
        //Description: Subtract (2’s complement method)
        //W register from register 'f'. If 'd' is 0,
        //the result is stored in the W register.
        //If 'd' is 1, the result is stored
        //back in register 'f'.

        protected override void execute(WorkingRegister W)
        {
            var result = W.Value.GetRegisterMap().RegisterList[f].Value - W.Value.Value;

            if(d)
            {
                W.Value.GetRegisterMap().RegisterList[f].Value = result;
            }
            else
            {
                W.Value.Value = result;
            }
        }

        public SUBWF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.d = d;

            execute(W);
        }
    }
}