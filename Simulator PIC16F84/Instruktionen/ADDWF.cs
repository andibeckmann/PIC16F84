using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84.Instruktionen
{
          public class ADDWF : BaseOperation
        {
            //ADDWF             Add W and f
            //--------------------------------------
            //Syntax:           [label] ADDWF f,d
            //Operands:         0 <= f <= 127
            //                  d e [0,1]
            //Operation:        (W) + (f) -> (destination)
            //Status Affected:  C, DC, Z
            //Description:      Add the contents of the W register
            //                  with register 'f'. If 'd' is 0, the result
            //                  is stored in the W register. If 'd' is
            //                  1, the result is stored back in
            //                  register 'f'.

            public ADDWF(int file, bool d, WorkingRegister W, RegisterFileMap Reg)
            {
                this.f = f;
                this.d = d;

                execute(W, Reg);
            }

            protected override void execute(WorkingRegister W, RegisterFileMap Reg)
            {
                var result = W.Value.Value + Reg.getRegister(f).Value;

                //Zero-Bit Logik
                if (result == 0)
                   Reg.SetZeroBit();
                else
                    Reg.ResetZeroBit();


                //Carry-Bit Logik
                if (result > 0xFF)
                    Reg.SetCarryBit();
                else
                    Reg.ResetCarryBit();

                //Unterscheidung Working Reg oder File Reg
                if (d)
                {
                    //Digit Carry Logik
                    if ((Reg.getRegister(f).Value & 0x0F) + W.Value.Value > 0x0f)
                        Reg.SetDigitCarryBit();
                    else
                       Reg.ResetDigitCarryBit();

                    //Resultat Ablegen
                   Reg.getRegister(f).Value = (byte)result;
                }
                else
                {
                    //Digit Carry Logik
                    if ((W.Value.Value & 0x0F) + Reg.getRegister(f).Value > 0x0f)
                       Reg.SetDigitCarryBit();
                    else
                       Reg.ResetDigitCarryBit();

                    //Resultat Ablegen
                    W.Value.Value = (byte)result;
                }
            }
    }
}
