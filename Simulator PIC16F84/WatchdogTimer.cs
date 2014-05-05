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

        public WatchdogTimer(RegisterFileMap Register)
        {
            timer = 0;
        }

        public void IncrementWatchdogTimer()
        {
             timer++;
        }

        public void ClearWatchdogTimer()
        {
            timer = 0;
        }


    }
}
