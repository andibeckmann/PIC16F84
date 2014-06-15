using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// ADDWF             Add W and f
    /// Syntax:           [label] ADDWF f,d
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   d e [0,1]
    /// Operation:        (W) + (f) -> (destination)
    /// Status Affected:  C, DC, Z
    /// Description:      Add the contents of the W register
    ///                   with register 'f'. If 'd' is 0, the result
    ///                   is stored in the W register. If 'd' is
    ///                   1, the result is stored back in
    ///                   register 'f'.
    /// </summary>
          public class ADDWF : BaseOperation
        {
           

            public ADDWF(int file, bool d, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
            {
                this.f = file;
                this.d = d;

                execute(W, Reg);
            }

            protected override void execute(WorkingRegister W, RegisterFileMap Reg)
            {
                var result = W.Value + Reg.getRegister(f).Value;

                /// Zero-Bit Logik
                if (result == 0 || result == 256)
                   Reg.SetZeroBit();
                else
                   Reg.ResetZeroBit();

                /// Carry-Bit Logik
                if (result > 0xFF)
                    Reg.SetCarryBit();
                else
                    Reg.ResetCarryBit();

                /// Digit Carry Logik
                if ((Reg.getRegister(f).Value & 0x0F) + (W.Value & 0x0F) > 0x0f)
                    Reg.SetDigitCarryBit();
                else
                    Reg.ResetDigitCarryBit();

                /// Resultat Ablegen, Unterscheidung Working Reg oder File Reg
                if (d)
                {
                    /// Sonderbehandlung PCL: Resultat muss auch auf den 13bit-Program Counter abgebildet werden, nicht nur auf PC-Reg
                    if (f == 0x02)
                    {
                        Reg.PC.Counter.Address = W.Value + derivePCAddress(Reg).Address;
                        return;
                    }
                    Reg.getRegister(f).Value = (byte)result;
                }

                else
                    W.Value = (byte)result;
            }
    }
}
