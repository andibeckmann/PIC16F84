using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    class ADDWF
    {
        public class ADDLW : BaseOperation
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

            public ADDLW(int k, WorkingRegister W)
            {
                Register = W.Value.GetRegisterMap();
                result = W.Value.Value + k;

                //Zero-Bit Logik
                if (result == 0)
                    Register.SetZeroBit();
                else
                    Register.ResetZeroBit();

                //Digit Carry Logik
                if ((W.Value.Value & 0x0F) + k > 0x0f)
                    Register.SetDigitCarryBit();
                else
                    Register.ResetDigitCarryBit();

                //Carry-Bit Logik
                if (result > 0xFF)
                    Register.SetCarryBit();
                else
                    Register.ResetCarryBit();
                W.Value.Value = result;

                MessageBox.Show("ADDLW detected! Added k=" + k.ToString() + " to the W-Register. Result: " + W.Value.Value.ToString() + ". Carry-Bit ist auf: " + W.Value.GetRegisterMap().getCarryBit().ToString());
            }
        }
    }
}
