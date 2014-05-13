using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class ProgramCounter
    {
        private ProgramMemoryAddress counter;
        private RegisterFileMap Reg;


        public ProgramCounter(RegisterFileMap _reg)
        {
            this.counter = new ProgramMemoryAddress(0);
            this.Reg = _reg;
        }

        public ProgramMemoryAddress Counter
        {
            get { return counter; }
            set {   this.counter = value;
                    this.Reg.getRegister(0x02).Value = getLower8Bits();
            }
        }

        public byte getLower8Bits()
        {
            return (byte)(Counter.Value & 0xFF);
        }

        public void InkrementPC()
        {
            Counter.Value++;
            this.Reg.getRegister(0x02).Value = getLower8Bits();
        }

        public void Clear()
        {
            counter.Value = 0;
        }
    }
}
