using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// INCF              Increment f
    /// Syntax:           [label] INCF f,d
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   d e [0,1]
    /// Operation:        (f) + 1 -> (destination)
    /// Status Affected:  Z
    /// Description:      The contents of register 'f' are
    ///                   incremented. If 'd' is 0, the result
    ///                   is placed in the W register. If 'd' is
    ///                   1, the result is placed back in register 'f'.
    /// </summary>
    public class INCF : BaseOperation
    {
        


        public INCF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            var result = Reg.getRegister(f).IncrementRegister();

            if (d)
                Reg.getRegister(f).Value = (byte)result;
            else
                W.Value = (byte)result;
            if( result == 0 )
                Reg.SetZeroBit();
        }
    }
}
