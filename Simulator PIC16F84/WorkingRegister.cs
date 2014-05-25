using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class WorkingRegister : RegisterByte
    {
        
        public WorkingRegister(int index) : base(index)
        {

        }

        //private RegisterByte value;

        //public WorkingRegister(RegisterFileMap RegisterMap)
        //{
        //    value = new RegisterByte(-1);
        //}

        //public RegisterByte Value
        //{
        //    get { return value; }
        //    set { this.value = value; }
        //}

        //public void ClearWorkingRegister()
        //{
        //    value.ClearRegister();
        //}
    }
}
