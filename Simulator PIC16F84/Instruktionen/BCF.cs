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

        public BCF(int f, int b, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.b = b;

            execute(Reg);
        }

        protected void execute(RegisterFileMap Reg)
        {
            Reg.getRegister(f).Value = (byte) TurnBitOff(Reg.getRegister(f).Value, b);
        }

        private static int TurnBitOff(int value, int b)
        {
            return (value & ~ (1 << b));
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }

    }
}
