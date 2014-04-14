using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterByte
    {
        private int content;
        public int Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }
    }

}
