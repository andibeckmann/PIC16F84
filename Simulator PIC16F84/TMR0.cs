using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    /// <summary>
    /// 8-bit Real-Time Clock/Counter
    /// </summary>
    public class TMR0
    {
        //TODO Was passiert wenn das Register per Quellcode und nicht per Timer geändert wird?

        private byte timer;
        private TimerStatus status;
        private int inhibitCycles;

        public delegate void TimerChangedHandler(object myObject,
                                             RegisterByte registerToChange);

        public event TimerChangedHandler TimerChanged;

        public TMR0()
        {
            this.timer = 0;
        }

        public byte Timer { 
            get
            {
                return timer;
            }
            set
            {
                
            }
                }

        public void SetTimerMode()
        {
            status = TimerStatus.TIMER;
        }

        public void SetCounterMode()
        {
            status = TimerStatus.COUNTER;
        }

        public void IncrementTimer()
        {
            switch (status)
            {
                case TimerStatus.COUNTER:
                    break;
                case TimerStatus.TIMER:
                    if(inhibitCycles == 0)
                    {
                        Timer++;
                        inhibitCycles = 3;
                    }
                    inhibitCycles--;
                    break;
                default:
                    break;
            }
        }

    }

    //public enum TimerStatus
    //{
    //    TIMER,
    //    COUNTER
    //}
}
