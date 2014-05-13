using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    /// <summary>
    /// 8-bit Real-Time Clock/Counter
    /// </summary>
    public class TMR0
    {
        private byte timer;

        public TMR0()
        {
            this.timer = 0;
        }
    }
}
