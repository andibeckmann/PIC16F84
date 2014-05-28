using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    class Timer0Module
    {

        private enum TimerMode { TIMER, COUNTER };
        private TimerMode timerMode;
        private RegisterByte tmr0Reg;
        private RegisterByte optionReg;
        private RegisterByte intconReg;
        private Prescaler prescaler;
        private int inhibitCycles;

        public Timer0Module(RegisterByte tmr0Reg, RegisterByte optionReg, RegisterByte intconReg, Prescaler prescaler)
        {
            this.tmr0Reg = tmr0Reg;
            this.prescaler = prescaler;
            this.optionReg = optionReg;
            this.intconReg = intconReg;
        }

        public void checkTimerMode()
        {
            //TMR0-Modes (timermode or countermode)
            if (optionReg.IsBitSet(5))
                setCounterMode();
            else
                setTimerMode();
        }

        public byte Timer
        {
            get
            {
                return tmr0Reg.Value;
            }
            set
            {
                inhibitCycles = 2;
                tmr0Reg.Value = value;
            }
        }

        public void setTimerMode()
        {
            timerMode = TimerMode.TIMER;
            //Clear Bit 5 in 81h
            if ( optionReg.IsBitSet(5) )
                optionReg.clearBit(5);
        }

        public void setCounterMode()
        {
            timerMode = TimerMode.COUNTER;
            //Set Bit 5 in 81h
            if (!optionReg.IsBitSet(5))
                optionReg.setBit(5);
        }

        public void incrementInCounterMode()
        {
            if (timerMode == TimerMode.COUNTER)
                tmr0Reg.IncrementRegister();
        }

        public void incrementInTimerMode()
        {
            if ( !(timerMode == TimerMode.TIMER) )
                return;
            if (inhibitCycles <= 0)
                if ( checkPrescaler() )
                    tmr0Reg.IncrementRegister();
            else
                inhibitCycles--;
        }

        private bool checkPrescaler()
        {
            if (prescaler.isAssignedToTMR0())
                if (!prescaler.IncrementPrescaler())
                    return false;
            return true;
        }
    }
}
