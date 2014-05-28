using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// CLRWDT Clear Watchdog Timer
    /// Syntax: [ label ] CLRWDT
    /// Operands: None
    /// Operation: 00h → WDT
    /// 0 → WDT prescaler,
    /// 1 → TO
    /// 1 → PD
    /// Status Affected: TO, PD
    /// Description: CLRWDT instruction resets the 
    /// Watchdog Timer. It also resets the 
    /// prescaler of the WDT. Status bits 
    /// TO and PD are set.
    /// </summary>
    public class CLRWDT : BaseOperation
    {
        

        public CLRWDT(WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            Reg.clearWatchdogTimer();
            Reg.clearPrescaler();
            //TODO Set TO,PD
            //W.Value.GetRegisterMap().
        }
    }
}
