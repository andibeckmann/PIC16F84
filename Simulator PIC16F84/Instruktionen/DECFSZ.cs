﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// DECFSZ            Decrement f, Skip if 0
    /// Syntax:           [label] DECFSZ f,d
    /// Operands:         0 &lt;= f &lt;= 127
    ///                   d e [0,1]
    /// Operation:        (f) - 1 -> (destination)
    ///                   skip if result = 0
    /// Status Affected:  None
    /// Description:      The contents of register 'f' are
    ///                   decremented. If 'd' is 0, the result
    ///                   is placed in the W register. If 'd' is
    ///                   1, the result is placed back in
    ///                   register 'f'.
    ///                   If the result is 1, the next instruc-
    ///                   tion is executed. If the result is 0,
    ///                   then a NOP is executed instead,
    ///                   making it a 2TCY instruction.
    /// </summary>
    public class DECFSZ : BaseOperation
    {
        public DECFSZ(int f, bool d, WorkingRegister W, RegisterFileMap Reg, ProgramCounter PC) : base(Reg)
        {
            this.f = f;
            this.d = d;

            execute(W, Reg, PC);
        }

        protected void execute(WorkingRegister W, RegisterFileMap Reg, ProgramCounter PC)
        {
            var result = Reg.getRegister(f,false).DecrementRegister();
            if (d)
            {
                /// Sonderbehandlung PCL: Resultat muss auch auf den 13bit-Program Counter abgebildet werden, nicht nur auf PC-Reg
                if (f == 0x02)
                {
                    Reg.PC.Counter.Address = derivePCAddress(Reg).Address - 1;
                }
                else
                Reg.getRegister(f,true).Value = (byte)result;
            }
            else
                W.Value = (byte)result;

            if (result == 0)
            {
                PC.Counter.Address++;
                NOP Operation = new NOP(Reg);
            }
         }




        protected override void execute(WorkingRegister W, RegisterFileMap Reg)
        {
            throw new NotImplementedException();
        }
    }
}
