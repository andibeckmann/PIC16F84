using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84.Instruktionen
{
    /// <summary>
    /// CALL              Call Subroutine
    /// Syntax:           [label] CALL k
    /// Operands:         0 &lt;= k &lt;= 2047
    /// Operation:        (PC) + 1 -> TOS,
    ///                   k -> PC&lt;10:0>,
    ///                   (PCLATH&lt;4:3>) -> PC&lt;12:11>
    /// Status Affected:  None
    /// Description:      Call Subroutine. First, return
    ///                   address (PC+1) is pushed onto
    ///                   the stack. The eleven-bit immedi-
    ///                   ate address is loaded into PC bits
    ///                   &lt;10:0>. the upper bits of the PC
    ///                   are loaded from PCLATH. CALL is
    ///                   a two-cycle instruction.
    /// </summary>
    public class CALL : BaseOperation
    {
        

        public CALL( byte k, ProgramCounter PC, Stack Stack, RegisterFileMap Reg ) : base(Reg)
        {
            this.k = k;

            execute(PC, Stack);
        }

    protected void execute(ProgramCounter PC, Stack Stack)
    {
        Stack.PushOntoStack(new ProgramMemoryAddress(DeriveReturnAddress(PC)));
        PC.Counter.Value = k - 1;   
        //TODO: PC LATCH Wasauchimmer
    }

    private int DeriveReturnAddress(ProgramCounter PC)
    {
        return PC.Counter.Value + 1;//TODO Value++ oder Value + 1? benötigt es eine eigene Methode?
    }

    protected override void execute(WorkingRegister W, RegisterFileMap Reg)
    {
        throw new NotImplementedException();
    }
    }
}
