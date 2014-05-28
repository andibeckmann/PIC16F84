using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class WatchdogTimer
    {
        private int timer;
        private RegisterFileMap Reg;
        private Prescaler prescaler;

        public WatchdogTimer(RegisterFileMap Reg, Prescaler prescaler)
        {
            timer = 0;
            this.Reg = Reg;
            this.prescaler = prescaler;
        }

        public void IncrementWatchdogTimer()
        {
            timer = timer + 1;
            checkTimeOutCondition();
        }

        private void checkTimeOutCondition()
        {
            if (timer > getWatchDogTimerLimit())
            {
                if (prescaler.isAssignedToWatchDogTimer())
                    checkForPrescalerTimeOut();
                else
                    WDTTimeOut();
                ClearWatchdogTimer();
            }
        }

        private void checkForPrescalerTimeOut()
        {
            if ( prescaler.IncrementPrescaler() )
                WDTTimeOut();
        }

        private int getWatchDogTimerLimit()
        {
            return 12000;
        }

        public void ClearWatchdogTimer()
        {
            timer = 0;
        }

        private void WDTTimeOut()
        {
            Reg.WDTTimeOut();
        }
    }
}
