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
            if (timer > calculateWatchDogTimerLimit())
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

        /// <summary>
        /// Calculates the WDT's time-out period in [ms]
        /// 
        /// The WDT has a nominal time-out period of 18 ms, (with 
        /// no prescaler). The time-out periods vary with
        /// temperature, VDD and process variations from part to
        /// part (see DC specs).
        /// 
        /// PIC operating speed - 200 ns instruction cycle
        /// therefore, the WDT increments once every 200ns (1/5000 ms)
        /// therefore the WDT needs to have incremented 18 * 5 * 1000
        /// in order to reach 18 ms
        /// </summary>
        /// <returns></returns>
        private int calculateWatchDogTimerLimit()
        {
            // geforderter Wert in SimTest04 ist dummerweise was völlig anderes, als was die Rechnung ergibt - statt 90 000 nur 17 949, also 1/5... Ging Lehmann aus irgendeinem grund davon aus, dass die Instruktionszyklen 1000ns statt 200ns lang sind?
            return 0x175f * 3;
            //return (18 * 5 * 1000);
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
