using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterByte
    {
        private int value;

        public RegisterByte()
        {
            value = 0;
        }

        public int Value {
            get { return value; }
            set { this.value = value;}
        }
        
    }

}
