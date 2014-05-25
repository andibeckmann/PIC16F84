using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class Prescaler
    {
        private byte counter;

        public byte Counter
        {
            get
            {
                return counter;
            }
            set
            {
                this.counter = value;
            }
        }

        public Prescaler()
        {
            counter = 0x00;
        }

        public void ClearPrescaler()
        {
            counter = 0x00;
        }
    }
}
