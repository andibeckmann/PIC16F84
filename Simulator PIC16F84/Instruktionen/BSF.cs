using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class BSF : BaseOperation
    {
        //BSF Bit Set f
        //Syntax: [label] BSF f,b
        //Operands: 0 ≤ f ≤ 127
        //0 ≤ b ≤ 7
        //Operation: 1 → (f<b>)
        //Status Affected: None
        //Description: Bit 'b' in register 'f' is set.

        public BSF(int f, int b, WorkingRegister W)
        {
            this.f = f;
            this.b = b;
            this.W = W;

            execute();   
        }

        private void execute()
        {
            var map = W.Value.GetRegisterMap();
            map.RegisterList[f].Value = TurnBitOn(map.RegisterList[f].Value, b);
        }

        public static int TurnBitOn(int value, int bitToTurnOn)
        {
            return (value | bitToTurnOn);
        }
    }
}
