﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// RLF               Rotate Left f through Carry
    /// Syntax:           [label] RLF f,d
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   d e [0,1]
    /// Operation:        See description below
    /// Status Affected:  C
    /// Description:      The contents of register ’f’ are
    ///                   rotated one bit to the left through
    ///                   the Carry Flag. If ’d’ is 0, the
    ///                   result is placed in the W register.
    ///                   If ’d’ is 1, the result is stored back
    ///                   in register ’f’.
    /// </summary>
    class RLF : BaseOperation
    {
        

        private int content;
        private bool CarryReminder;

        public RLF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;
            CarryReminder = false;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            content = Reg.getRegister(f,false).Value;
            var temp = content;
            if ((temp & 0x80) == 0x80)
            {
                CarryReminder = true;
            }

            content = content << 1;
            content = content & 0xFF;

            if (Reg.getCarryBit())
            {
                content = content | 0x01;
            }

            if (d)
            {
                Reg.getRegister(f,true).Value = Convert.ToByte(content);
            }
            else
            {
                W.Value = (byte)content;
            }

            if (CarryReminder)
            {
                Reg.SetCarryBit();
            }
            else
            {
                Reg.ResetCarryBit();
            }

        }
    }
}
