using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class SLEEP : BaseOperation
    {
        //SLEEP
        //--------------------------------------
        //Syntax:           [label]  SLEEP
        //Operands:         None
        //Operation:        00h -> WDT
        //                  0 -> WDT prescaler
        //                  1 -> !TO
        //                  0 -> !PD
        //Status Affected:  !TO, !PD
        //Description:      The power-down status bit, PD is
        //                  cleared. Time-out status bit, TO
        //                  is set. Watchdog Timer and its
        //                  prescaler are cleared.
        //                  The processor is put into SLEEP
        //                  mode with the oscillator stopped.

        public SLEEP(WorkingRegister W, WatchdogTimer WDT, Prescaler Prescaler)
        {
            execute(W, WDT, Prescaler);
        }

        private void execute(WorkingRegister W, WatchdogTimer WDT, Prescaler Prescaler)
        {
            W.Value.GetRegisterMap().ResetPowerDownBit();
            W.Value.GetRegisterMap().SetTimeOutBit();

            WDT.ClearWatchdogTimer();
            Prescaler.ClearPrescaler();

            //TODO: Put processor into SLEEP mode and stop oscillator

        }

        protected override void execute(WorkingRegister W)
        {
            throw new NotImplementedException();
        }
    }
}
