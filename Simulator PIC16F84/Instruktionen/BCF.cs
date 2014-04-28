using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    public class BCF
    {
        //BCF Bit Clear f
        //Syntax: [label] BCF f,b
        //Operands: 0 ≤ f ≤ 127
        //0 ≤ b ≤ 7
        //Operation: 0 → (f<b>)
        //Status Affected: None
        //Description: Bit 'b' in register 'f' is cleared

        public BCF(WorkingRegister W)
        {

        }
    }
}
