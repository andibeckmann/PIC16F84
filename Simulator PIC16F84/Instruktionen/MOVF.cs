﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// MOVF              Move f,d
    /// Syntax:           [label] IMOVF f,d
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   d e [0,1]
    /// Operation:        (f) -> (destination)
    /// Status Affected:  Z
    /// Description:      The contents of register f are
    ///                   moved to a destination dependant
    ///                   upon the status of d. If d = 0, destination
    ///                   is W register. If d = 1, the
    ///                   destination is file register f itself.
    ///                   d = 1 is useful to test a file register,
    ///                   since status flag Z is affected.
    /// </summary>
    public class MOVF : BaseOperation
    {
        


        public MOVF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            var content = Reg.getRegister(f,false).Value;

            if (d)
               Reg.getRegister(f,true).Value = content;
            else
                W.Value = content;

            if (content == 0)
                Reg.SetZeroBit();
            else
                Reg.clearZeroBit();
        }
    }
}
