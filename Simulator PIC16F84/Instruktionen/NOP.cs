using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// NOP               No Operation
    /// Syntax:           [label] NOP
    /// Operands:         None
    /// Operation:        No operation
    /// Status Affected:  None
    /// Description:      No operation
    /// </summary>
    public class NOP : BaseOperation
    {
       

        public NOP(RegisterFileMap Reg) : base(Reg)
        {
            execute(null, null);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
           ;
        }
    }
}
