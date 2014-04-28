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

        public Addlw(int literal, WorkingRegister WorkingRegister)
        {
            int result;

            result = WorkingRegister.Value + literal;

            MessageBox.Show("ADDLW detected! Added W=" + WorkingRegister.Value.ToString() + " to k=" + literal.ToString());
        }

    }
}
