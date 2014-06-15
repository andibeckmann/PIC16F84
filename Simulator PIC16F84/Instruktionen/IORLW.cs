using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// IORLW Inclusive OR Literal with W
    /// Syntax: [ label ] IORLW k
    /// Operands: 0 &lt; k &lt; 255
    /// Operation: (W) .OR. k -> (W)
    /// Status Affected: Z
    /// Description: The contents of the W register are
    /// OR’ed with the eight-bit literal 'k'.
    /// The result is placed in the W
    /// register.
    /// </summary>
    class IORLW : BaseOperation
    {
        

        public IORLW(byte k, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;
            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            Reg.SetZeroBit();

            W.Value |= (byte)k;
        }

        
    }
}
