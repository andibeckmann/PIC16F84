using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class InterruptService
    {
        private RegisterFileMap Reg;
        private byte intInterruptOldValue;
        private byte portBInterruptOldValue;


        public InterruptService(RegisterFileMap Reg)
        {
            this.Reg = Reg;
            this.intInterruptOldValue = 0;
            this.portBInterruptOldValue = 0;
        }

        public void executeRoutine()
        {
            if (isThereAnInterruptRequest())
            {
                if (Reg.isInPowerDownMode())
                {
                    Reset newReset = new Reset(Reg);
                    newReset.resetInterruptWakeUp();
                    Reg.setPowerDownBit();
                }
                if (isGlobalInterruptEnableBitSet())
                    executeInterruptServiceRoutine();
            }
        }

        private void executeInterruptServiceRoutine()
        {
            clearGlobalInterruptEnableBit();
            Reg.getStack().PushOntoStack(new ProgramMemoryAddress(Reg.PC.DeriveReturnAddress()));
            Reg.PC.Counter.Address = 0x04 - 1;
        }

        private bool isGlobalInterruptEnableBitSet()
        {
            return Reg.getIntconRegister().isBitSet(7);
        }

        public bool isThereAnInterruptRequest()
        {
            return (isThereAnIntInterruptRequest() || isThereATimer0InterruptRequest() || isThereAPortBInterruptRequest() || isThereAPDataEEPROMInterruptRequest());
        }

        private bool isThereAnIntInterruptRequest()
        {
            return (isIntInterruptFlagBitSet() && isIntInterruptEnabled());
        }

        private bool isThereATimer0InterruptRequest()
        {
            return (isTimer0InterruptFlagSet() && isTimer0OverflowInterruptEnabled());
        }

        private bool isThereAPortBInterruptRequest()
        {
            return (isRBInterruptEnabled() && isRBInterruptFlagSet());
        }

        private bool isThereAPDataEEPROMInterruptRequest()
        {
            return (isEEPROMInterruptEnabled() && isEEPROMInterruptFlagSet());
        }

        private bool isEEPROMInterruptFlagSet()
        {
            return Reg.getEECON1().isBitSet(4);
        }

        private bool isTimer0InterruptFlagSet()
        {
            return Reg.getIntconRegister().isBitSet(2);
        }

        private bool isEEPROMInterruptEnabled()
        {
            return Reg.getIntconRegister().isBitSet(6);
        }

        private bool isTimer0OverflowInterruptEnabled()
        {
            return Reg.getIntconRegister().isBitSet(5);
        }

        private bool isIntInterruptEnabled()
        {
            return Reg.getIntconRegister().isBitSet(4);
        }

        private bool isIntEdgBitSet()
        {
            return Reg.getOptionRegister().isBitSet(6);
        }

        private bool isIntInterruptFlagBitSet()
        {
            return Reg.getIntconRegister().isBitSet(1);
        }

        private bool isRBInterruptEnabled()
        {
            return Reg.getIntconRegister().isBitSet(3);
        }

        private bool isRBInterruptFlagSet()
        {
            return Reg.getIntconRegister().isBitSet(0);
        }

        private void setRBInterruptFlag()
        {
            Reg.getIntconRegister().setBit(0);
        }

        public void setGlobalInterruptEnableBit()
        {
            Reg.getIntconRegister().setBit(7);
        }

        private void clearGlobalInterruptEnableBit()
        {
            Reg.getIntconRegister().clearBit(7);
        }

        /// <summary>
        /// INT INTERRUPT
        /// 
        /// External interrupt on RB0/INT pin is edge triggered:
        /// either rising if INTEDG bit (OPTION_REG<6>) is set,
        /// or falling if INTEDG bit is clear.
        /// When a valid edge
        /// appears on the RB0/INT pin, the INTF bit
        /// (INTCON<1>) is set. This interrupt can be disabled by
        /// clearing control bit INTE (INTCON<4>).
        /// </summary>
        public void checkForIntInterrupt()
        {
            if (isIntInterruptEnabled())
            {
                if (isIntEdgBitSet())
                {
                    if (Reg.getBRegister(false).checkForRisingEdge(intInterruptOldValue, 0))
                        setIntFBit();
                }
                else
                {
                    if (Reg.getBRegister(false).checkForFallingEdge(intInterruptOldValue, 0))
                        setIntFBit();
                }
            }
            intInterruptOldValue = Reg.getBRegister(false).Value;
        }

        /// <summary>
        /// PORTB INTERRUPT
        /// 
        /// An input change on PORTB<7:4> sets flag bit RBIF
        /// (INTCON<0>). The interrupt can be enabled/disabled
        /// by setting/clearing enable bit RBIE (INTCON<3>)
        /// (Section 4.2).
        /// </summary>
        public void checkForPortBInterrupt()
        {
            if (isRBInterruptEnabled())
                if (checkForPortBInterruptInputChange())
                    setRBInterruptFlag();
        }

        /// <summary>
        /// PORT B INTERRUPT CONDITION
        /// 
        /// The Port B interrupt is triggered by input changes
        /// on PORTB<7:4>.
        /// For Port B to work as input, the corresponding bit
        /// in the TRISB-Register needs to be set to 1.
        /// </summary>
        /// <returns></returns>
        private bool checkForPortBInterruptInputChange()
        {
            byte changedBits = (byte)(portBInterruptOldValue ^ Reg.getBRegister(false).Value);
            byte inputChange = (byte)(changedBits & Reg.getTRISB().Value);
            if ((inputChange & 0xF0) != 0x00)
            {
                portBInterruptOldValue = Reg.getBRegister(false).Value;
                return true;
            }
            return false;
        }

        private void setIntFBit()
        {
            Reg.getIntconRegister().setBit(1);
        }

        private void clearIntFBit()
        {
            Reg.getIntconRegister().clearBit(1);
        }
    }
}
