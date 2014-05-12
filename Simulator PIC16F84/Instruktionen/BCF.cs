using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class BCF : BaseOperation
    {
        //BCF Bit Clear f
        //Syntax: [label] BCF f,b
        //Operands: 0 ≤ f ≤ 127
        //0 ≤ b ≤ 7
        //Operation: 0 → (f<b>)
        //Status Affected: None
        //Description: Bit 'b' in register 'f' is cleared

        public BCF(int f, int b, WorkingRegister W)
        {
            this.f = f;
            this.b = b;

            execute(W);
        }

        protected override void execute(WorkingRegister W)
        {
            W.Value.GetRegisterMap().RegisterList[f].Value = (byte) TurnBitOff(W.Value.GetRegisterMap().RegisterList[f].Value, b);
        }

        private static int TurnBitOff(int value, int bitToTurnOff)
        {
            return (value & ~bitToTurnOff);
        }
    }
}
