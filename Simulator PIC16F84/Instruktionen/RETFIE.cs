using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// RETFIE Return from Interrupt
    /// Syntax: [ label ] RETFIE
    /// Operands: None
    /// Operation: TOS -> PC,
    /// 1 -> GIE
    /// Status Affected: None
    /// </summary>
    public class RETFIE : BaseOperation
    {
        
        public RETFIE(ProgramCounter PC, Stack stack, RegisterFileMap Reg) : base(Reg)
        {
            execute(Reg, PC, stack);
        }

        protected void execute(RegisterFileMap Reg, ProgramCounter PC, Stack stack)
        {
            PC.Counter.Value = stack.PullFromStack().Value - 1;
            Reg.setGlobalInterruptEnableBit();
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
