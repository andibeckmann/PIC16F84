using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class SWAPF : BaseOperation
    {
        //SWAPF Swap Nibbles in f
        //Syntax: [ label ] SWAPF f,d
        //Operands: 0 < f < 127
        //d e [0,1]
        //Operation: (f<3:0>) -> (destination<7:4>),
        //(f<7:4>) -> (destination<3:0>)
        //Status Affected: None
        //Description: The upper and lower nibbles of
        //register 'f' are exchanged. If 'd' is
        //0, the result is placed in W register.
        //If 'd' is 1, the result is placed in
        //register 'f'.
        protected override void execute(WorkingRegister W)
        {
            var fileRegister = W.Value.GetRegisterMap().getRegisterList[f].Value;
            var result = ((fileRegister >> 4) & 0x0f) | ((fileRegister << 4) & 0xf0);

            if(d)
            {
                W.Value.GetRegisterMap().getRegisterList[f].Value = result;
            }
            else
            {
                W.Value.Value = result;
            }
        }

        public SWAPF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            this.d = d;

            execute(W);
        }
    }
}
