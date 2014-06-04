using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// RRF               Rotate Right f through Carry
    /// Syntax:           [label] RRF f,d
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   d e [0,1]
    /// Operation:        See description below
    /// Status Affected:  C
    /// Description:      The contents of register ’f’ are
    ///                   rotated one bit to the right through
    ///                   the Carry Flag. If ’d’ is 0, the
    ///                   result is placed in the W register.
    ///                   If ’d’ is 1, the result is stored back
    ///                   in register ’f’.
    /// </summary>
    class RRF : BaseOperation
    {
        

        private int content;
        private bool CarryReminder;

        public RRF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;
            CarryReminder = false;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            content = Reg.getRegister(f).Value;
            if ( (content & 0x1) == 0x1 )
                CarryReminder = true;

            content = content>>1;
            content = content & 0xFF;

            if (Reg.getCarryBit())
                content += 0x80;

            if (d)
                Reg.getRegister(f).Value = (byte)content;
            else
                W.Value = (byte)content;

            if (CarryReminder)
                Reg.SetCarryBit();
            else
                Reg.ResetCarryBit();

        }
    }
}
