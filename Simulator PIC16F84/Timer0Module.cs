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
        private byte internalCount;

        public Timer0Module(RegisterByte tmr0Reg, RegisterByte optionReg, RegisterByte intconReg, Prescaler prescaler)
        {
            this.tmr0Reg = tmr0Reg;
            this.prescaler = prescaler;
            this.optionReg = optionReg;
            this.intconReg = intconReg;
            this.internalCount = tmr0Reg.Value;
        }

        public void checkTimer()
        {
            //When assigned to the Tmr0-Module, all instruction writing on the Tmr0-Register will clear the prescaler
            if (prescaler.isAssignedToTMR0() && internalCount != tmr0Reg.Value)
            {
                prescaler.clearPrescaler();
                internalCount = tmr0Reg.Value;
            }
        }

        public void checkTimerMode()
        {
            //TMR0-Modes (timermode or countermode)
            if (optionReg.isBitSet(5))
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
                internalCount = value;
                if ( prescaler.isAssignedToTMR0() )
                    prescaler.clearPrescaler();
            }
        }

        public void setTimerMode()
        {
            timerMode = TimerMode.TIMER;
            //Clear Bit 5 in 81h
            if ( optionReg.isBitSet(5) )
                optionReg.clearBit(5);
        }

        public void setCounterMode()
        {
            timerMode = TimerMode.COUNTER;
            //Set Bit 5 in 81h
            if (!optionReg.isBitSet(5))
                optionReg.setBit(5);
        }

        public void incrementInCounterMode()
        {
            if (timerMode == TimerMode.COUNTER)
                IncrementWithPrescaler();
        }

        private void IncrementWithPrescaler()
        {
            if (prescalerConditionFulfilled())
            {
                checkOverflowcondition();
                tmr0Reg.IncrementRegister();
                internalCount = (byte)(internalCount + 1);
            }
        }

        private void checkOverflowcondition()
        {
            if ( tmr0Reg.Value == 255 && intconReg.isBitSet(7) && intconReg.isBitSet(5))
                setTimer0InterruptFlagBit();
        }

        private void setTimer0InterruptFlagBit()
        {
            intconReg.setBit(2);
        }

        public void incrementInTimerMode()
        {
            if (inhibitCycles <= 0)
                IncrementWithPrescaler();
            else
                inhibitCycles--;
        }

        private bool prescalerConditionFulfilled()
        {
            if (prescaler.isAssignedToTMR0())
                if (!prescaler.IncrementPrescaler())
                    return false;
            return true;
        }

        public bool isInCounterMode()
        {
            if (timerMode == TimerMode.COUNTER)
                return true;
            else
                return false;
        }
    }
}
