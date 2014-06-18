using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class Reset
    {
        private RegisterFileMap Reg;

        public Reset(RegisterFileMap Reg)
        {
            this.Reg = Reg;
        }

        /// <summary>
        /// RESET
        /// Power-on Reset (POR)
        /// The PIC16F84A differentiates between various kinds of RESET,
        /// one of which is the Power-on Reset (POR).
        /// 
        /// RESET CONDITION FOR PROGRAM COUNTER AND THE STATU REGISTER
        /// Program Counter: 000h
        /// STATUS Register: 0001 1xxx
        /// 
        /// Legend: u = unchanged, x = unknown
        /// </summary>
        public void resetPowerOn()
        {
            Reg.clearProgramCounter();
            setStatusForPOR();
            resetConditionsPowerOnReset();
        }

        /// <summary>
        /// RESET
        /// MCLR during normal operation / MCLR during SLEEP
        /// The PIC16F84A differentiates between various kinds of RESET,
        /// one of which is triggered with the MCLR Pin, which can occur either:
        /// -during normal operation
        /// -during SLEEP
        /// 
        /// RESET CONDITION FOR PROGRAM COUNTER AND THE STATU REGISTER
        /// Program Counter: 000h
        /// STATUS Register: 000u uuuu (during normal operation)
        /// STATUS Register: 0001 0uuu (during SLEEP)
        /// 
        /// Legend: u = unchanged, x = unknown
        /// </summary>
        public void resetMCLR()
        {
            Reg.clearProgramCounter();
            if (Reg.isInPowerDownMode())
                setStatusForMCLRSleep();
            else
                setStatusForMCLRNormal();
            resetConditionsMCLROrWDTNormal();
        }

        /// <summary>
        /// RESET
        /// WDT Reset (during normal operation) / WDT Wake-up (during SLEEP)
        /// The PIC16F84A differentiates between various kinds of RESET,
        /// one of which is the Watchdog-Timer Reset (WDT), which can occur either:
        /// -during normal operation
        /// -as a Wake-up during SLEEP
        /// 
        /// RESET CONDITION FOR PROGRAM COUNTER AND THE STATU REGISTER
        /// Program Counter: 000h (during normal operation)
        /// Program Counter: PC + 1 (Wake-Up)
        /// STATUS Register: 0000 1xxx (during normal operation)
        /// STATUS Register: uuu0 0xxx (Wake-Up)
        /// 
        /// Legend: u = unchanged, x = unknown
        /// </summary>
        public void resetWDT()
        {
            if (Reg.isInPowerDownMode())
            {
                //PC + 1 : Will already be incremented by ExecuteCycle
                //Reg.PC.InkrementPC();
                setStatusForWDTWakeUp();
                resetConditionsWakeUp();
            }
            else
            {
                Reg.clearProgramCounter();
                setStatusForWDTNormal();
                resetConditionsMCLROrWDTNormal();
            }
        }

        /// <summary>
        /// RESET
        /// Interrupt wake-up from SLEEP
        /// The PIC16F84A differentiates between various kinds of RESET,
        /// one of which is the Interrupt wake-up from SLEEP.
        /// 
        /// RESET CONDITION FOR PROGRAM COUNTER AND THE STATU REGISTER
        /// Program Counter: PC + 1
        /// STATUS Register: uuu1 0uuu
        /// 
        /// Legend: u = unchanged, x = unknown
        /// </summary>
        public void resetInterruptWakeUp()
        {
            //PC + 1 : Will already be incremented by ExecuteCycle
            //Reg.PC.InkrementPC();
            setStatusForInterruptWakeUp();
            resetConditionsWakeUp();
        }

        /// <summary>
        /// RESET CONDITIONS FOR ALL REGISTERS
        /// Power-on Reset
        /// 
        /// W Register      : xxxx xxxx
        /// INDF            : ---- ----
        /// TMR0            : xxxx xxxx
        /// PCL             : done separately
        /// STATUS          : done separately
        /// FSR             : xxxx xxxx
        /// PORTA           : ---x xxxx
        /// PORTB           : xxxx xxxx
        /// EEDATA          : xxxx xxxx
        /// EEADR           : xxxx xxxx
        /// PCLATH          : ---0 0000
        /// INTCON          : 0000 000x
        /// INDF            : ---- ----
        /// OPTION_REG      : 1111 1111
        /// PCL             : s.a.
        /// STATUS          : s.a.
        /// FSR             : s.a.
        /// TRISA           : ---1 1111
        /// TRISB           : 1111 1111
        /// EECON1          : ---0 x000
        /// EECON2          : ---- ----
        /// PCLATH          : s.a.
        /// INTCON          : s.a.
        /// </summary>
        private void resetConditionsPowerOnReset()
        {
            clearPCLATH();
            setIntconForPOR();
            Reg.getOptionRegister().fillRegister();
            Reg.getTRISA().fillRegister();
            Reg.getTRISB().fillRegister();
            setEecon1ForPOR();
        }

        /// <summary>
        /// RESET CONDITIONS FOR ALL REGISTERS
        /// MCLR during:
        /// -normal operation
        /// -SLEEP
        /// WDT Reset during normal operation
        /// 
        /// W Register      : uuuu uuuu
        /// INDF            : ---- ----
        /// TMR0            : uuuu uuuu
        /// PCL             : done separately
        /// STATUS          : done separately
        /// FSR             : uuuu uuuu
        /// PORTA           : ---u uuuu
        /// PORTB           : uuuu uuuu
        /// EEDATA          : uuuu uuuu
        /// EEADR           : uuuu uuuu
        /// PCLATH          : ---0 0000
        /// INTCON          : 0000 000u
        /// INDF            : ---- ----
        /// OPTION_REG      : 1111 1111
        /// PCL             : s.a.
        /// STATUS          : s.a.
        /// FSR             : s.a.
        /// TRISA           : ---1 1111
        /// TRISB           : 1111 1111
        /// EECON1          : special handling
        /// EECON2          : ---- ----
        /// PCLATH          : s.a.
        /// INTCON          : s.a.
        /// </summary>
        private void resetConditionsMCLROrWDTNormal()
        {
            clearPCLATH();
            setIntconForPOR();
            Reg.getOptionRegister().fillRegister();
            Reg.getTRISA().fillRegister();
            Reg.getTRISB().fillRegister();
            setEecon1ForPOR();
        }

        /// <summary>
        /// RESET CONDITIONS FOR ALL REGISTERS
        /// Wake-up from SLEEP:
        /// -through interrupt
        /// -through WDT Time-out
        /// 
        /// W Register      : uuuu uuuu
        /// INDF            : ---- ----
        /// TMR0            : uuuu uuuu
        /// PCL             : done separately
        /// STATUS          : done separately
        /// FSR             : uuuu uuuu
        /// PORTA           : ---u uuuu
        /// PORTB           : uuuu uuuu
        /// EEDATA          : uuuu uuuu
        /// EEADR           : uuuu uuuu
        /// PCLATH          : ---u uuuu
        /// INTCON          : uuuu uuuu
        /// INDF            : ---- ----
        /// OPTION_REG      : uuuu uuuu
        /// PCL             : s.a.
        /// STATUS          : s.a.
        /// FSR             : s.a.
        /// TRISA           : ---u uuuu
        /// TRISB           : uuuu uuuu
        /// EECON1          : ---0 uuuu
        /// EECON2          : ---- ----
        /// PCLATH          : s.a.
        /// INTCON          : s.a.
        /// </summary>
        private void resetConditionsWakeUp()
        {
            setEecon1ForSLEEP();
        }

        private void setStatusForPOR()
        {
            int newValue = 0x07 & Reg.getStatusRegister().Value;
            newValue = 0x18 | newValue;
            Reg.getStatusRegister().Value = (byte)newValue;
        }

        private void setStatusForMCLRNormal()
        {
            Reg.getStatusRegister().Value = (byte) (0x1F & Reg.getStatusRegister().Value);
        }

        private void setStatusForMCLRSleep()
        {
            int newValue = 0x07 & Reg.getStatusRegister().Value;
            newValue = 0x10 | newValue;
            Reg.getStatusRegister().Value = (byte)newValue;
        }

        private void setStatusForWDTNormal()
        {
            int newValue = 0x07 & Reg.getStatusRegister().Value;
            newValue = 0x08 | newValue;
            Reg.getStatusRegister().Value = (byte)newValue;
        }

        private void setStatusForWDTWakeUp()
        {
            Reg.getStatusRegister().Value = (byte) (0xE7 & Reg.getStatusRegister().Value);
        }

        private void setStatusForInterruptWakeUp()
        {
            int newValue = 0xE7 & Reg.getStatusRegister().Value;
            newValue = 0x10 | newValue;
            Reg.getStatusRegister().Value = (byte)newValue;
        }

        private void clearPCLATH()
        {
            Reg.getPCLATH().ClearRegister();
        }

        private void setIntconForPOR()
        {
            Reg.getIntconRegister().Value = (byte) ( 0x01 & Reg.getIntconRegister().Value);
        }

        private void setEecon1ForPOR()
        {
            int newValue = 0x08 & Reg.getEECON1().Value;
            Reg.getEECON1().Value = (byte)newValue;
        }

        private void setEecon1ForSLEEP()
        {
            int newValue = 0x0F & Reg.getEECON1().Value;
            Reg.getEECON1().Value = (byte)newValue;
        }
    }
}
