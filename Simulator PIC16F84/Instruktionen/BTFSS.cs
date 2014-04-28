﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class BTFSS : BaseOperation
    {
        //BTFSS Bit Test f, Skip if Set
        //Syntax: [label] BTFSS f,b
        //Operands: 0 ≤ f ≤ 127
        //0 ≤ b ≤ 7
        //Operation: skip is(f<b>) = 1
        //Status Affected: None
        //Description: If bit 'b' in register 'f' is '0', the next
        //instruction is executed.
        //If Bit 'b' is '1', then the next instruction is discarded 
        //and a NOP is executed instead, making this a 2TCY instruction.

        public BTFSS(int f, int b, WorkingRegister W)
        {
            this.f = f;
            this.b = b;
            this.W = W;

            execute(); 
        }

        private void execute()
        {
            var map = W.Value.GetRegisterMap();
            if( IsBitSet(map.RegisterList[f].Value, b))
            {
                //TODO
                //Setze Zeile/PC whatever eins hoch
                //Execute NOP
            }
        }
    }
}
