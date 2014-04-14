using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class ProgramMemoryMap
    {
        public ProgramMemoryByte[] ProgramMemory;

        public ProgramMemoryMap()
        {
            ProgramMemory = new ProgramMemoryByte[1024];
            for (int var = 0; var < ProgramMemory.Length; var++)
            {
                ProgramMemory[var] = new ProgramMemoryByte();
            }
        }
    }
}
