using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterFileMap
    {
        private RegisterByte[] registerList;
        public int[] mappingArray;
        private Timer0Module timer0;
        private WatchdogTimer WDT;
        private Prescaler prescaler;
        private byte portAOldValue;
        private enum InterruptSource { INT, TMR0, PortB, DataEEPROM };
        private Stack stack;
        private ProgramMemoryAddress ConfigurationBits;
        public ProgramCounter PC { get; set; }

        public RegisterFileMap(Stack stack, ProgramMemoryAddress ConfigurationBits)
        {
            this.PC = new ProgramCounter(this);
            this.stack = stack;
            this.ConfigurationBits = ConfigurationBits;
            fillMappingArray();
            registerList = new RegisterByte[256];
            portAOldValue = 0;
            for (int var = 0; var < registerList.Length; var++ )
            {
                registerList[var] = new RegisterByte(var);
            }

            //Timer0 MOdule, Watchdogtimer und Prescaler
            prescaler = new Prescaler(getTMR0Register(), getOptionRegister());
            WDT = new WatchdogTimer(this, prescaler);
            timer0 = new Timer0Module(getTMR0Register(), getOptionRegister(), getIntconRegister(), prescaler);
        }


        private void setUpExtraRegisterViews()
        {

        }

        public bool isTimeOutBitSet()
        {
            return getStatusRegister().isBitSet(4);
        }

        public void ClearWatchdogTimer()
        {
            WDT.ClearWatchdogTimer();
        }

        public void WDTTimeOut()
        {
            clearTimeOutBit();
        }

        private void clearTimeOutBit()
        {
            getStatusRegister().clearBit(4);
        }

        public void setTimeOutBit()
        {
            getStatusRegister().setBit(4);
        }

        public void checkOptionRegisterSettings()
        {
            prescaler.checkPrescalerSettings();
            timer0.checkTimerMode();
        }

        public void checkTimerRegister()
        {
            timer0.checkTimer();
        }

        private void fillMappingArray()
        {
            mappingArray = new int[256];
            for (int index = 0; index < 255; index++)
            {
                    mappingArray[index] = index;
            }
            FillMappingArrayExceptions();
        }

        private void FillMappingArrayExceptions()
        {
            mappingArray[0x80] = 0x00;
            mappingArray[0x82] = 0x02;
            mappingArray[0x83] = 0x03;
            mappingArray[0x84] = 0x04;
            mappingArray[0x8A] = 0x0A;
            mappingArray[0x8B] = 0x0B;
        }

        private RegisterByte readINDFReg()
        {
            var address = getFSRReg();
            if (address == 0)
                return registerList[0];
            else
                return getRegister(address);
        }

        private int getFSRReg()
        {
            return registerList[0x04].Value;
        }

        public RegisterByte[] getRegisterList {
            get { return registerList; }
            set { this.registerList = value;}
        }

        /// <summary>
        /// Gibt ein Register anhand des Index zurück
        /// </summary>
        /// <param name="index">Gibt die Speicherzelle des Byte an</param>
        /// <returns>gewählte Speicherzelle</returns>
        public RegisterByte getRegister(int index)
        {
            if (index == 0)
                return readINDFReg();
            else if (isRegisterBankSelectBitSet() && index < 0x80)
                index = mappingArray[index + 0x80];
            else
                index = mappingArray[index];
            return this.registerList[index];
        }

        public bool timer0InCounterMode()
        {
            return timer0.isInCounterMode();
        }

        public RegisterByte getTMR0Register()
        {
            return registerList[1];
        }

        public RegisterByte getStatusRegister()
        {
            return registerList[3];
        }

        private void setStatusToWDTReset()
        {
            registerList[3].Value = (byte) (registerList[3].Value & 0x08);
        }

        public RegisterByte getARegister()
        {
            return registerList[5];
        }

        public RegisterByte getBRegister()
        {
            return registerList[6];
        }

        public RegisterByte getOptionRegister()
        {
            return registerList[0x81];
        }

        /// <summary>
        /// Interrupt Control Register (Intcon)
        /// </summary>
        /// <returns></returns>
        public RegisterByte getIntconRegister()
        {
            return registerList[0x0B];
        }

        private void setIntconToWDTReset()
        {
            registerList[0x0B].Value = (byte)(registerList[0x0B].Value & 0x0E);
        }

        internal void SetCarryBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value | 0x01);
        }

        internal void ResetCarryBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value & 0xFE);
        }

        public bool getCarryBit()
        {
            if ( (registerList[3].Value & 0x01) == 0x01)
                return true;
            else
                return false;
        }

        internal void SetDigitCarryBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value | 0x02);
        }

        internal void ResetDigitCarryBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value & 0xFD);
        }

        public bool getDigitCarryBit()
        {
            if ((registerList[3].Value & 0x02) == 0x02)
                return true;
            else
                return false;
        }

        internal void SetZeroBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value | 0x04);
        }

        internal void ResetZeroBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value & 0xFB);
        }

        public bool getZeroBit()
        {
            if ((registerList[3].Value & 0x02) == 0x04)
                return true;
            else
                return false;
        }

        public void SetPowerDownBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value | 0x08);
        }

        public void ResetPowerDownBit()
        {
            registerList[3].Value = (byte) (registerList[3].Value & 0xF7);
        }

        private void clearPCLATH()
        {
            registerList[0x0A].Value = 0x00;
        }

        public void Init()
        {
            this.getRegister(0x02).Value = 0x00; 
            this.getRegister(0x03).Value = 0x18;
            this.getRegister(0x07).Value = 0x00;
            this.getRegister(0x0A).Value = 0x00;
            this.getRegister(0x0B).Value = 0x00;
            this.getRegister(0x81).Value = 0xFF;
            this.getRegister(0x85).Value = 0x1F;
            this.getRegister(0x86).Value = 0xFF;
            this.getRegister(0x87).Value = 0x00;
            this.getRegister(0x88).Value = 0x00;
        }

        public void ClearRegister()
        {
            foreach (var registerByte in registerList)
            {
                registerByte.Value = 0;
            }
        }

        public bool isRegisterBankSelectBitSet()
        {
            if ((registerList[3].Value & 0x20) == 0x20)
                return true;
            else
                return false;
        }

        public void RegisterContentChanged(object sender, RegisterByte registerToChange)
        {
            registerList.Where(item => item.Index == registerToChange.Index).Select(item => item.Value = registerToChange.Value);
        }

        public void instructionCycleTimeElapsed()
        {
            if ( !timer0.isInCounterMode() ) 
                timer0.incrementInTimerMode();
           
            if (isWatchdogTimerEnabled())
            {
                incrementWatchdogTimer();
            }
        }

        private bool isWatchdogTimerEnabled()
        {
            return ConfigurationBits.isBitSet(1);
        }

        public void incrementCounter()
        {
            timer0.incrementInCounterMode();       
        }

        public void EnableTimerInterrupt()
        {
            getIntconRegister().setBit(5);
        }

        public void DisableTimerInterrupt()
        {
            getIntconRegister().clearBit(5);
        }

        public void clearWatchdogTimer()
        {
            WDT.ClearWatchdogTimer();
        }

        public void clearWatchdogPrescaler()
        {
            if( !prescaler.isAssignedToTMR0() )
                prescaler.clearPrescaler();
        }

        public void checkForFallingAndRisingEdgesOnPortA()
        {
            //Check for Rising or Falling Edges for Timer 0 Module Counter Mode
            if (getOptionRegister().isBitSet(4))
            {
                if (getARegister().checkForFallingEdge(portAOldValue, 4))
                    incrementCounter();
            }
            else
            {
                if (getARegister().checkForRisingEdge(portAOldValue, 4))
                    incrementCounter();
            }
             portAOldValue = getARegister().Value;
        }

        public void checkForInterrupt()
        {
            if ( !isGlobalInterruptEnableBitSet() )
                return;

            if (isThereAnIntInterruptRequest())
                interruptServiceRoutine(InterruptSource.INT);
            else if (isThereATimer0InterruptRequest())
                interruptServiceRoutine(InterruptSource.TMR0);
            else if (isThereAPortBInterruptRequest())
                interruptServiceRoutine(InterruptSource.PortB);
            else if (isThereAPDataEEPROMInterruptRequest())
                interruptServiceRoutine(InterruptSource.DataEEPROM);
            else
                return;
        }

        private bool isGlobalInterruptEnableBitSet()
        {
            return getIntconRegister().isBitSet(7);
        }

        private bool isThereAnIntInterruptRequest()
        {
            //TODO: Implementation of INT Interrupt
            return false;
        }

        private bool isThereATimer0InterruptRequest()
        {
            return ( isTimer0OverflowInterruptEnabled() && isTimer0OverflowInterruptFlagBitSet());
        }

        private bool isThereAPortBInterruptRequest()
        {
            //TODO: Implementation of PortB Interrupt
            return false;
        }

        private bool isThereAPDataEEPROMInterruptRequest()
        {
            //TODO: Implementation of DataEEPROM Interrupt
            return false;
        }

        private bool isTimer0OverflowInterruptFlagBitSet()
        {
            return getIntconRegister().isBitSet(2);
        }

        private bool isTimer0OverflowInterruptEnabled()
        {
            return getIntconRegister().isBitSet(5);
        }

        public void setGlobalInterruptEnableBit()
        {
            getIntconRegister().setBit(7);
        }

        private void clearGlobalInterruptEnableBit()
        {
            getIntconRegister().clearBit(7);
        }

        private void interruptServiceRoutine(InterruptSource interruptSource)
        {
            clearGlobalInterruptEnableBit();
            stack.PushOntoStack(new ProgramMemoryAddress(DeriveReturnAddress(PC)));
            PC.Counter.Address = 0x04;
        }

        public int DeriveReturnAddress(ProgramCounter PC)
        {
            return PC.Counter.Address + 1;
        }

        public void incrementWatchdogTimer()
        {
            WDT.IncrementWatchdogTimer();
        }

        public void WatchDogTimerReset()
        {
            PC.Clear();
            setStatusToWDTReset();
            clearPCLATH();
            setIntconToWDTReset();
            getOptionRegister().Value = 0xFF;
            getTRISA().Value = 0x1F;
            getTRISB().Value = 0xFF;
            getEECON1().Value = 0x00;
        }

        public RegisterByte getTRISA()
        {
            return registerList[0x85];
        }

        public RegisterByte getTRISB()
        {
            return registerList[0x86];
        }

        public RegisterByte getEECON1()
        {
            return registerList[0x88];
        }
      
        /// <summary>
        /// Setzt ein bestimmtes Bit in einem Byte.
        /// </summary>
        /// <param name="b">Byte, welches bearbeitet werden soll.</param>
        /// <param name="BitNumber">Das zu setzende Bit (0 bis 7).</param>
        /// <returns>Ergebnis - Byte</returns>
        public static byte SetBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        /// <summary>
        /// Löscht ein bestimmtes Bit in einem Byte.
        /// </summary>
        /// <param name="b">Byte, welches bearbeitet werden soll.</param>
        /// <param name="BitNumber">Das zu löschende Bit (0 bis 7).</param>
        /// <returns>Ergebnis - Byte</returns>
        public static byte ClearBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b & (byte)(~(0x01 << BitNumber)));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }
    } 
}
