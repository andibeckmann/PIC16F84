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

            private int result;
            private RegisterFileMap Register;
            private RegisterByte f;
            private RegisterByte W;

            public ADDWF(int file, bool d, WorkingRegister WReg)
            {
                W = WReg.Value;
                Register = W.GetRegisterMap();
                f = Register.RegisterList[file];

                result = W.Value + f.Value;

                //Zero-Bit Logik
                if (result == 0)
                    Register.SetZeroBit();
                else
                    Register.ResetZeroBit();


                //Carry-Bit Logik
                if (result > 0xFF)
                    Register.SetCarryBit();
                else
                    Register.ResetCarryBit();

                MessageBox.Show("ADDWF detected! Added content of register f = " + f.Value.ToString() + " and content of W-Register W = " + W.Value.ToString() + ". Result: " + result.ToString() + ". Desintation-Bit ist auf: " + d.ToString());

                //Unterscheidung Working Reg oder File Reg
                if (d)
                {
                    //Digit Carry Logik
                    if ((f.Value & 0x0F) + W.Value > 0x0f)
                        Register.SetDigitCarryBit();
                    else
                        Register.ResetDigitCarryBit();

                    //Resultat Ablegen
                    f.Value = result;
                }
                else
                {
                    //Digit Carry Logik
                    if ((W.Value & 0x0F) + f.Value > 0x0f)
                        Register.SetDigitCarryBit();
                    else
                        Register.ResetDigitCarryBit();

                    //Resultat Ablegen
                    W.Value = result;
                }
            }
    }
}
