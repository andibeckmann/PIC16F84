using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator_PIC16F84
{
    public class RegisterFileMap
    {
        public RegisterByte[] RegisterList;

        public RegisterFileMap()
        {
            RegisterList = new RegisterByte[256];
            for (int var = 0; var < RegisterList.Length; var++ )
            {
                RegisterList[var] = new RegisterByte();
            }
        }

        public RegisterByte[] getRegisterList {
            get { return RegisterList; }
            set { this.RegisterList = value;}
        }

        //er.... ???
        public RegisterByte getStatusRegisterContent()
        {
            return RegisterList[3];
        }        
    
    }
}
