using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84
{
    public partial class RunTimeCounter : Form
    {
        public RunTimeCounter()
        {
            InitializeComponent();
        }
        
        public void UpdateLabel(int newValue)
        {
            var label = this.Controls.Find("Value", true).FirstOrDefault() as Label;
            label.Text = newValue.ToString();
        }
    }
}
