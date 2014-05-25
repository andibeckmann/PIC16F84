using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// SLEEP
    /// Syntax:           [label]  SLEEP
    /// Operands:         None
    /// Operation:        00h -> WDT
    ///                   0 -> WDT prescaler
    ///                   1 -> !TO
    ///                   0 -> !PD
    /// Status Affected:  !TO, !PD
    /// Description:      The power-down status bit, PD is
    ///                   cleared. Time-out status bit, TO
    ///                   is set. Watchdog Timer and its
    ///                   prescaler are cleared.
    ///                   The processor is put into SLEEP
    ///                   mode with the oscillator stopped.
    /// </summary>
    class SLEEP : BaseOperation
    {
        

        public SLEEP(WorkingRegister W, RegisterFileMap Reg, WatchdogTimer WDT, Prescaler Prescaler) : base(Reg)
        {
            execute(W, Reg, WDT, Prescaler);
        }

        private void execute(WorkingRegister W, RegisterFileMap Reg, WatchdogTimer WDT, Prescaler Prescaler)
        {
            Reg.ResetPowerDownBit();
            Reg.SetTimeOutBit();

            WDT.ClearWatchdogTimer();
            Prescaler.ClearPrescaler();

            //TODO: Put processor into SLEEP mode and stop oscillator

        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
