using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class Stack
    {
        private List<ProgramMemoryAddress> StackAddresses;


        public Stack()
        {
            StackAddresses = new List<ProgramMemoryAddress>();
            for (int var = 0; var < 8; var++)
                StackAddresses.Add(new ProgramMemoryAddress(0));
        }

        public void PushOntoStack(int ReturnAddress)
        {
            StackAddresses.Insert(0, new ProgramMemoryAddress(ReturnAddress));
            StackAddresses.RemoveAt(8);
        }

        public ProgramMemoryAddress PullFromStack()
        {
            var topOfStack = StackAddresses[0];
            for (int i = 0; i < 7; i++ )
            {
                StackAddresses[i] = StackAddresses[i + 1];
            }
            StackAddresses[7].Value = 0; 
            return topOfStack;
        }
    }
}
