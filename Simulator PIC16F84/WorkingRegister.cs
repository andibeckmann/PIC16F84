﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class WorkingRegister
    {

        private RegisterByte value;

        public WorkingRegister(RegisterFileMap RegisterMap)
        {
            value = new RegisterByte(RegisterMap);
        }

        public RegisterByte Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}