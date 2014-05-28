using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class Prescaler
    {
        private byte counter;
        private RegisterByte tmr0Reg;
        private RegisterByte optionReg;
        private enum prescalerAssignment { TMR0, WDT };
        private prescalerAssignment prescalerMode;
        private byte prescalerRate;

        public Prescaler(RegisterByte tmr0Reg, RegisterByte optionReg)
        {
            counter = 0x00;
            this.tmr0Reg = tmr0Reg;
            this.optionReg = optionReg;
            checkPrescalerSettings();
        }

        public void checkPrescalerSettings()
        {
            setPrescalerAssignment();
            setPrescalerRate();
        }

        public bool isAssignedToTMR0()
        {
            if (prescalerMode == prescalerAssignment.TMR0)
                return true;
            else
                return false;
        }

        public void clearPrescaler()
        {
            counter = 0x00;
        }

        public bool IncrementPrescaler()
        {
            counter = (byte) (counter + 1);
            return isIncrementConditionForTimerMet();
        }

        private bool isIncrementConditionForTimerMet()
        {
            if (counter % prescalerRate == 0)
                return true;
            else
                return false;
        }

        private void setPrescalerAssignment()
        {
                    if (isPrescalerAssignmentBitSet())
                        assignPrescalerToWDT();
                    else
                        assignPrescalerToTMR0();
        }

        private bool isPrescalerAssignmentBitSet()
        {
            return optionReg.IsBitSet(3);
        }

        private void assignPrescalerToWDT()
        {
            prescalerMode = prescalerAssignment.WDT;
        }

        private void assignPrescalerToTMR0()
        {
            prescalerMode = prescalerAssignment.TMR0;
        }

        private void setPrescalerRate()
        {
            prescalerRate = (byte)Math.Pow(2, determinePrescalerRateExponent());
        }

        private byte determinePrescalerRateExponent()
        {
            if (prescalerMode == prescalerAssignment.TMR0)
                return (byte) (getPrescalerSelectBits() + 1);
            else
                return getPrescalerSelectBits();
        }

        private byte getPrescalerSelectBits()
        {
            return (byte)(optionReg.Value & 0x07);
        }
    }
}