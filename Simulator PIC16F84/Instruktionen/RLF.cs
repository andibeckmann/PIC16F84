using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class RLF : BaseOperation
    {
        //RLF               Rotate Left f through Carry
        //--------------------------------------
        //Syntax:           [label] RLF f,d
        //Operands:         0 <= f <= 127
        //                  d e [0,1]
        //Operation:        See description below
        //Status Affected:  C
        //Description:      The contents of register ’f’ are
        //                  rotated one bit to the left through
        //                  the Carry Flag. If ’d’ is 0, the
        //                  result is placed in the W register.
        //                  If ’d’ is 1, the result is stored back
        //                  in register ’f’.

        private int content;
        private RegisterFileMap Register;
        private bool CarryReminder;

        public RLF(int f, bool d, WorkingRegister W)
        {
            this.f = f;
            CarryReminder = false;
            Register = W.Value.GetRegisterMap();

            execute(W);
        }

        protected override void execute(WorkingRegister W)
        {
            content = Register.RegisterList[f].Value;
            if ( (content & 0x80) == 0x80 )
                CarryReminder = true;

            content = content<<1;
            content = content & 0xFF;

            if (Register.getCarryBit())
                content += 1;

            if (d)
                W.Value.GetRegisterMap().RegisterList[f].Value = (sbyte)content;
            else
                W.Value.Value = (sbyte)content;

            if (CarryReminder)
                W.Value.GetRegisterMap().SetCarryBit();
            else
                W.Value.GetRegisterMap().ResetCarryBit();

        }
    }
}
