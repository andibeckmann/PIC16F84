using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// Instruction Set Basisklasse
    /// </summary>
    public abstract class BaseOperation
    {
        /// <summary>
        /// Register file address (0x00 to 0x7F)
        /// </summary>
        protected int f;

        /// <summary>
        /// Working register (accumulator)
        /// </summary>
        protected WorkingRegister W;

        /// <summary>
        /// /Bit address within an 8-bit file register
        /// </summary>
        protected int b;

        /// <summary>
        /// Literal field, constant data or label
        /// </summary>
        protected int k;

        /// <summary>
        /// Destination select;
        /// d = 0:store result in W
        /// d = 1: store result in file register f.
        /// Default is d = 1
        /// </summary>
        protected bool d;

        /// <summary>
        /// Program Counter
        /// </summary>
        protected ProgramCounter PC;

        /// <summary>
        /// Time-out bit
        /// </summary>
        protected bool TO;

        /// <summary>
        /// Power-down bit
        /// </summary>
        protected bool PD;

        /// <summary>
        /// Stack-Returnadressen des Program Counters
        /// </summary>
        protected Stack Stack;

        public BaseOperation(RegisterFileMap Reg)
        {
            Reg.instructionCycleTimeElapsed();
        }

        /// <summary>
        /// method to execute the instruction
        /// </summary>
        /// <param name="W">WorkingRegister</param>
        /// <param name="Reg">RegisterFileMap</param>
        protected abstract void execute(WorkingRegister W, RegisterFileMap Reg);

        /// <summary>
        /// Check if Bit is set in Byte
        /// </summary>
        /// <param name="byteValue">Byte</param>
        /// <param name="bit">Bit</param>
        /// <returns>result</returns>
        protected bool IsBitSet(int byteValue, int bit)
        {
            return (byteValue & (1 << bit)) != 0;
        }

        /// <summary>
        /// Turn Bit on
        /// </summary>
        /// <param name="byteValue">Byte</param>
        /// <param name="bit">Bit</param>
        /// <returns>Byte with Bit turned on</returns>
        protected static byte TurnBitOn(int byteValue, int bit)
        {
            return (byte)(byteValue | 1 << bit);
        }

        /// <summary>
        /// Turn Bit off
        /// </summary>
        /// <param name="byteValue">Byte</param>
        /// <param name="bit">Bit</param>
        /// <returns>Byte with Bit turned off</returns>
        protected static byte TurnBitOff(int byteValue, int bit)
        {
            return (byte) (byteValue & ~(1 << bit));
        }

        /// <summary>
        ///     /// Operation:  (PC) + 1 -> TOS,
        ///                     k -> PC&lt;10:0>,
        ///                     CLATH&lt;4:3>) -> PC&lt;12:11>
        /// </summary>
        protected ProgramMemoryAddress deriveAddress(RegisterFileMap Reg)
        {
            int lower11Bits = k & 0x7FF;
            int upper2Bits = (Reg.getPCLATH().Value & 0x18)>>3;
            int combinedAddress = (upper2Bits << 11) + lower11Bits;

            return new ProgramMemoryAddress(combinedAddress);
        }

        /// <summary>
        ///     /// Operation:  (PC) + 1 -> TOS,
        ///                     k -> PC&lt;10:0>,
        ///                     CLATH&lt;4:3>) -> PC&lt;12:11>
        /// </summary>
        protected ProgramMemoryAddress derivePCAddress(RegisterFileMap Reg)
        {
            int lower8Bits = Reg.getRegister(0x02).Value;
            int upper5Bits = Reg.getPCLATH().Value & 0x1F;
            int combinedAddress = (upper5Bits << 8) + lower8Bits;

            return new ProgramMemoryAddress(combinedAddress);
        }
    }
}
