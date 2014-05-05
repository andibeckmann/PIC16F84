﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class IORWF : BaseOperation
    {
        //IORWF Inclusive OR W with f
        //Syntax: [ label ] IORWF f,d
        //Operands: 0 < f < 127
        //d e [0,1]
        //Operation: (W) .OR. (f) -> (destination)
        //Status Affected: Z
        //Description: Inclusive OR the W register with
        //register 'f'. If 'd' is 0, the result is
        //placed in the W register. If 'd' is 1,
        //the result is placed back in
        //register 'f'.

        public IORWF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.d = d;

            execute();
        }

        protected override void execute()
        {
            var result = W.Value.Value | W.Value.GetRegisterMap().RegisterList[f].Value;
 
            if(d)
            {
                W.Value.GetRegisterMap().RegisterList[f].Value = result;
            }
            else
            {
                W.Value.Value = result;
            }
        }
    }
}