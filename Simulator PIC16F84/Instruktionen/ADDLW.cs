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


        public ADDLW(byte k, WorkingRegister W, RegisterFileMap Reg) : base(Reg)
        {
            this.k = k;

            execute(W, Reg);
        }

        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            var result = W.Value.Value + k;
            //Zero-Bit Logik
            if (result == 0)
                Reg.SetZeroBit();
            else
               Reg.ResetZeroBit();

            //Digit Carry Logik
            if ((W.Value.Value & 0x0F) + k > 0x0f)
                Reg.SetDigitCarryBit();
            else
                Reg.ResetDigitCarryBit();

            //Carry-Bit Logik
            if (result > 0xFF)
                Reg.SetCarryBit();
            else
                Reg.ResetCarryBit();
            W.Value.Value = (byte)result;
        }

    }
}
