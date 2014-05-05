using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84.Instruktionen
{
    public class ADDLW : BaseOperation
    {
        //ADDLW             Add Literal and W
        //--------------------------------------
        //Syntax:           [label] ADDLW k
        //Operands:           0 <= k < 255
        //Operation:        (W) + k -> (W)
        //Status Affected:  C, DC, Z
        //Description:      The contents of the W register
        //                  are added to the eight-bit literal 'k'
        //                  and the result is placed in the W
        //                  register.

        private int result;
        private RegisterFileMap Register;

        public ADDLW(int k, WorkingRegister W)
        {
            this.k = k;
            this.W = W;
            this.Register = W.Value.GetRegisterMap();
            this.result = W.Value.Value + k;

            execute();
        }

        protected override void execute()
        {
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

            //           MessageBox.Show("ADDLW detected! Added k=" + k.ToString() +" to the W-Register. Result: " + W.Value.Value.ToString() + ". Carry-Bit ist auf: " + W.Value.GetRegisterMap().getCarryBit().ToString() );
        }

    }
}
