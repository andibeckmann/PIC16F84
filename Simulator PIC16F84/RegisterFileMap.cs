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
        private InterruptService interruptServiceRoutine;
        public int[] mappingArray;
        private Timer0Module timer0;
        private WatchdogTimer WDT;
        private Prescaler prescaler;
        private Stack stack;
        private ProgramMemoryAddress ConfigurationBits;
        private EEPROM EepromMemory;
        public ProgramCounter PC { get; set; }
        private RegisterByte portALatch;
        private RegisterByte portBLatch;

        public RegisterFileMap()
        {
            InitConfigurationBits();
            this.PC = new ProgramCounter(this);
            stack = new Stack();
            fillMappingArray();
            createRegisterMap();
            createEepromMemory();
            this.interruptServiceRoutine = new InterruptService(getIntconRegister(), getBRegister(false), getOptionRegister(), getEECON1(), getTRISB(), stack, PC);

            //Timer0 MOdule, Watchdogtimer und Prescaler
            prescaler = new Prescaler(getTMR0Register(), getOptionRegister());
            WDT = new WatchdogTimer(prescaler, getStatusRegister());
            timer0 = new Timer0Module(getTMR0Register(), getARegister(false), getOptionRegister(), getIntconRegister(), prescaler);
            portALatch = new RegisterByte(-0x05);
            portALatch.RegisterChanged += portAIOFunction;
            portBLatch = new RegisterByte(-0x06);
            portBLatch.RegisterChanged += portBIOFunction;
        }

        /// <summary>
        /// The configuration bits can be programmed (read as '0'),
        /// or left unprogrammed (read as '1'), to select various
        /// device configurations. These bits are mapped in
        /// program memory location 2007h.
        /// Address 2007h is beyond the user program memory
        /// space and it belongs to the special test/configuration
        /// memory space (2000h - 3FFFh): This space can only
        /// be accessed during programming.
        /// </summary>
        private void InitConfigurationBits()
        {
            ConfigurationBits = new ProgramMemoryAddress(0);
            //FOSC1:FOSC0: Oscillator Selection bits - 11 = RC oscillator
            // 11 = RC oscillator
            // 10 = HS oscillator
            // 01 = XT oscillator
            // 00 = LP oscillator
            ConfigurationBits.setBit(0);
            ConfigurationBits.setBit(1);
            //WDTE: Watchdog Timer Enable bit
            // 1 = WDT enabled
            // 0 = WDT disabled
            ConfigurationBits.setBit(2);
            //PWRTE: Power-up Timer Enable bit
            // 1 = Power-up Timer is disabled
            // 0 = Power-up Timer is enabled
            ConfigurationBits.setBit(3);
            //CP: Code Protection bit (bits 4-13)
            // 1 = Code protection disabled
            // 0 = All program memory is code protected
            ConfigurationBits.Address = ConfigurationBits.Address | 0x1FF0;
        }

        private void createRegisterMap()
        {
            this.registerList = new RegisterByte[256];
            for (int var = 0; var < registerList.Length; var++)
            {
                registerList[var] = new RegisterByte(var);
            }
        }

        private void createEepromMemory()
        {
            this.EepromMemory = new EEPROM(getEECON1(),getEECON2(),getEEADR(),getEEDATA());
        }

        public EEPROM getEepromMemory()
        {
            return EepromMemory;
        }

        public void checkEEPROMFunctionality()
        {
            EepromMemory.checkEEPROMFunctionality();
        }

        public bool isTimeOutBitSet()
        {
            return getStatusRegister().isBitSet(4);
        }

        public void ClearWatchdogTimer()
        {
            WDT.ClearWatchdogTimer();
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

        private RegisterByte readINDFReg(bool write)
        {
            var address = getFSRReg();
            if (address == 0)
                return registerList[0];
            else
                return getRegister(address,write);
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
        public RegisterByte getRegister(int index, bool write, bool ignoreBankSelection)
        {
            if (index == 0)
                return readINDFReg(write);
            else if (!ignoreBankSelection && isRegisterBankSelectBitSet() && index < 0x80)
                index = mappingArray[index + 0x80];
            else if (index == 0x05)
                return getARegister(write);
            else if (index == 0x06)
                return getBRegister(write);
            else
                index = mappingArray[index];
            return this.registerList[index];
        }

        public RegisterByte getRegister(int index, bool write)
        {
            return getRegister(index, write, false);
        }

        public bool timer0InCounterMode()
        {
            return timer0.isInCounterMode();
        }

        public void checkCountingConditions()
        {
            timer0.checkForFallingAndRisingEdgesOnPortA();
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

        public RegisterByte getARegister(bool write)
        {
            if (write)
                return portALatch;
            else
                return registerList[5];
        }

        private void portAIOFunction(object sender, int index)
        {
            RegisterByte output = new RegisterByte(-2);
            for (int position = 0; position < 8; position++)
            {
                if (!getARegister(false).isBitImplemented(position))
                    continue;
                if (!getTRISA().isBitSet(position))
                    writeFromLatchIntoPort(getARegister(false), output, position);
                else
                    showInput(portALatch, output, position);
            }
            getARegister(false).Value = output.Value;
        }

        private void showInput(RegisterByte register, RegisterByte output, int position)
        {
            if (register.isBitSet(position))
                output.setBit(position);
            else
                output.clearBit(position);
        }

        private void writeFromLatchIntoPort(RegisterByte latch, RegisterByte output, int position)
        {
            if (latch.isBitSet(position))
                output.setBit(position);
            else
                output.clearBit(position);
        }

        private void portBIOFunction(object sender, int index)
        {
            RegisterByte output = new RegisterByte(-2);
            for (int position = 0; position < 8; position++)
            {
                if (!getBRegister(false).isBitImplemented(position))
                    continue;
                if (!getTRISB().isBitSet(position))
                    writeFromLatchIntoPort(getBRegister(false), output, position);
                else
                    showInput(portBLatch, output, position);
            }
            getBRegister(false).Value = output.Value;
        }

        public RegisterByte getBRegister(bool write)
        {
            if (write)
                return portBLatch;
            else
                return this.registerList[0x06];
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

        public void setPowerDownBit()
        {
            getStatusRegister().setBit(3);
        }

        public void clearPowerDownBit()
        {
            getStatusRegister().clearBit(3);
        }

        private void clearPCLATH()
        {
            registerList[0x0A].Value = 0x00;
        }

        public void Init()
        {
            this.getRegister(0x02,true).Value = 0x00;
            this.getRegister(0x03, true).Value = 0x18;
            this.getRegister(0x07, true).Value = 0x00;
            this.getRegister(0x0A, true).Value = 0x00;
            this.getRegister(0x0B, true).Value = 0x00;
            this.getRegister(0x81, true).Value = 0xFF;
            this.getRegister(0x85, true).Value = 0x1F;
            this.getRegister(0x86, true).Value = 0xFF;
            this.getRegister(0x87, true).Value = 0x00;
            this.getRegister(0x88, true).Value = 0x00;
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

        public void incrementWatchdogTimer()
        {
            WDT.IncrementWatchdogTimer();
        }

        /// <summary>
        /// Watchdog Timer Reset (during normal operation)
        /// The PIC16F84A differentiates between various kinds of RESET, one of which is the WDT Reset.
        /// Reset conditions for all registers during WDT Reset are:
        /// W Register      : uuuu uuuu
        /// INDF            : ---- ----
        /// TMR0            : uuuu uuuu
        /// PCL             : 0000 0000
        /// STATUS          : 0000 1uuu
        /// FSR             : uuuu uuuu
        /// PORTA           : ---u uuuu
        /// PORTB           : uuuu uuuu
        /// EEDATA          : uuuu uuuu
        /// EEADR           : uuuu uuuu
        /// PCLATH          : ---0 0000
        /// INTCON          : 0000 000u
        /// INDF            : ---- ----
        /// OPTION_REG      : 1111 1111
        /// PCL             : 0000 0000
        /// STATUS          : 0000 1uuu
        /// FSR             : uuuu uuuu
        /// TRISA           : ---1 1111
        /// TRISB           : 1111 1111
        /// EECON1          : ---0 q000
        /// EECON2          : ---- ----
        /// PCLATH          : ---0 0000
        /// INTCON          : 0000 000u
        /// </summary>
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

        public RegisterByte getPCLATH()
        {
            return registerList[0x0A];
        }

        public RegisterByte getEECON1()
        {
            return registerList[0x88];
        }

        public RegisterByte getEECON2()
        {
            return registerList[0x89];
        }

        public RegisterByte getEEDATA()
        {
            return registerList[0x08];
        }

        public RegisterByte getEEADR()
        {
            return registerList[0x09];
        }

        public void checkForInterrupt()
        {
            interruptServiceRoutine.executeRoutine();
        }

        public void checkForIntInterrupt()
        {
            interruptServiceRoutine.checkForIntInterrupt();
        }

        public void checkForPortBInterrupt()
        {
            interruptServiceRoutine.checkForPortBInterrupt();
        }

        public void setGlobalInterruptEnableBit()
        {
            interruptServiceRoutine.setGlobalInterruptEnableBit();
        }

        public void setTimeOutBit()
        {
            WDT.setTimeOutBit();
        }

        public Stack getStack()
        {
            return stack;
        }

        public bool isInPowerDownMode()
        {
            return !getStatusRegister().isBitSet(3);
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
