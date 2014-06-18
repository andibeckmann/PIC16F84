using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// SUBWF Subtract W from f
    /// Syntax: [ label ] SUBWF f,d
    /// Operands: 0 &lt; f &lt; 127
    /// d e [0,1]
    /// Operation: (f) - (W) -> (destination)
    /// Status Affected: C, DC, Z
    /// Description: Subtract (2’s complement method)
    /// W register from register 'f'. If 'd' is 0,
    /// the result is stored in the W register.
    /// If 'd' is 1, the result is stored
    /// back in register 'f'.
    /// </summary>
    class SUBWF : BaseOperation
    {
        public SUBWF(int f, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            var alterWert = Reg.getRegister(f, false).Value;
            var subtraktor = W.Value;
            var result = alterWert + ~subtraktor + 1;

            /// Zero-Bit Logik
            if (result == 0 || result == 256)
                Reg.SetZeroBit();
            else
                Reg.ResetZeroBit();

            /// Carry-Bit Logik
            if (result <  alterWert && result >= 0)
                Reg.SetCarryBit();
            else
                Reg.ResetCarryBit();

            /// Digit Carry Logik
            if ((alterWert & 0x0F) + (subtraktor & 0x0F) > 0x0f)
                Reg.SetDigitCarryBit();
            else
                Reg.ResetDigitCarryBit();

            /// Resultat ablegen, Unterscheidung Working Reg oder File Reg
            if (d)
            {
                /// Sonderbehandlung PCL: Resultat muss auch auf den 13bit-Program Counter abgebildet werden, nicht nur auf PC-Reg
                if (f == 0x02)
                {
                    Reg.PC.Counter.Address = derivePCAddress(Reg).Address - W.Value;
                }
                else
                Reg.getRegister(f,true).Value = (byte)result;
            }
            else
            {
                W.Value = (byte)result;
            }
        }
    }
}
