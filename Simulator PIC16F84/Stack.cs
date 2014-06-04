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
        public event EventHandler StackChanged;

        public Stack()
        {
            StackAddresses = new List<ProgramMemoryAddress>();
            for (int var = 0; var < 8; var++)
                StackAddresses.Add(new ProgramMemoryAddress());
        }

        public void PushOntoStack(ProgramMemoryAddress returnAddress)
        {
            StackAddresses.Insert(0, returnAddress);
            StackAddresses.RemoveAt(8);
            this.StackChanged(this, EventArgs.Empty);
        }

        public ProgramMemoryAddress PullFromStack()
        {
            var topOfStack = StackAddresses[0];
            for (int i = 0; i < 7; i++ )
            {
                StackAddresses[i] = StackAddresses[i + 1];
            }
            StackAddresses[7].Address = 0;
            this.StackChanged(this, EventArgs.Empty);
            return topOfStack;
        }

        public ProgramMemoryAddress getStackValues(int index)
        {
            return StackAddresses[index];
        }

        public ProgramMemoryAddress Value
        {
            get { return PullFromStack(); }
            set
            {
                PushOntoStack(value);
            }
        }

        public void ClearStack()
        {
            foreach (var item in StackAddresses)
	        {
                item.Address = 0;
	        }
        }
    }
}
