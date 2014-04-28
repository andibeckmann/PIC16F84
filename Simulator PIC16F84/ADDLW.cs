using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84
{
    public class Addlw : BaseOperation
    {
        //ADDLW             Add Literal and W
        //Syntax:           0 <= k < 255
        //Operation:        (W) + k -> (W)
        //Status Affected:  C, DC, Z
        //Description:      The contents of the W register
        //                  are added to the eight-bit literal 'k'
        //                  and the result is placed in the W
        //                  register.

        public Addlw(int k, WorkingRegister W)
        {
            int result;

            result = W.Value.Value + k;
            W.Value.Value = result;

            MessageBox.Show("ADDLW detected! Added k=" + k.ToString() +" to the W-Register. Result: " + W.Value.Value.ToString() + ". Carry-Bit ist auf: " + W.Value.GetRegisterMap().getCarryBit().ToString() );
        }

    }
}
