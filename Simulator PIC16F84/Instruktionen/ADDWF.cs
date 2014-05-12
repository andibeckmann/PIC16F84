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

            public ADDWF(int file, bool d, WorkingRegister W)
            {
                this.f = f;
                this.d = d;

                execute(W);
            }

            protected override void execute(WorkingRegister W)
            {
                var result = W.Value.Value + W.Value.GetRegisterMap().RegisterList[f].Value;

                //Zero-Bit Logik
                if (result == 0)
                    W.Value.GetRegisterMap().SetZeroBit();
                else
                    W.Value.GetRegisterMap().ResetZeroBit();


                //Carry-Bit Logik
                if (result > 0xFF)
                    W.Value.GetRegisterMap().SetCarryBit();
                else
                    W.Value.GetRegisterMap().ResetCarryBit();

                //                MessageBox.Show("ADDWF detected! Added content of register f = " + f.Value.ToString() + " and content of W-Register W = " + W.Value.ToString() + ". Result: " + result.ToString() + ". Destintation-Bit ist auf: " + d.ToString());

                //Unterscheidung Working Reg oder File Reg
                if (d)
                {
                    //Digit Carry Logik
                    if ((W.Value.GetRegisterMap().RegisterList[f].Value & 0x0F) + W.Value.Value > 0x0f)
                        W.Value.GetRegisterMap().SetDigitCarryBit();
                    else
                        W.Value.GetRegisterMap().ResetDigitCarryBit();

                    //Resultat Ablegen
                    W.Value.GetRegisterMap().RegisterList[f].Value = (byte)result;
                }
                else
                {
                    //Digit Carry Logik
                    if ((W.Value.Value & 0x0F) + W.Value.GetRegisterMap().RegisterList[f].Value > 0x0f)
                        W.Value.GetRegisterMap().SetDigitCarryBit();
                    else
                        W.Value.GetRegisterMap().ResetDigitCarryBit();

                    //Resultat Ablegen
                    W.Value.Value = (byte)result;
                }
            }
    }
}
