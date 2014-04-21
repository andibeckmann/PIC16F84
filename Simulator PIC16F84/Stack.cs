using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class Stack
    {
        private ProgramMemoryAddress[] StackAddresses;


        public Stack()
        {
            StackAddresses = new ProgramMemoryAddress[8];
            for (int var = 0; var < StackAddresses.Length; var++)
            {
                StackAddresses[var] = new ProgramMemoryAddress(0);
            }
        }
    }
}
