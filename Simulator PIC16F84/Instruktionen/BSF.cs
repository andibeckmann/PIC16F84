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

        public BSF(int f, int b, RegisterFileMap Reg)
        {
            this.f = f;
            this.b = b;

            execute(Reg);   
        }

        private void execute(RegisterFileMap Reg)
        {
            Reg.getRegister(f).Value = TurnBitOn(Reg.getRegister(f).Value, b);
        }

        public static byte TurnBitOn(int value, int bitToTurnOn)
        {
            return (byte)(value | bitToTurnOn);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }

    }
}
