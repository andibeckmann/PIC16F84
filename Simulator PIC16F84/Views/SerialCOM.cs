using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84.Views
{
    public partial class SerialCOM : Form
    {
        private Main Pic;

        public SerialCOM(Main Pic)
        {
            InitializeComponent();
            this.Pic = Pic;
            comboBoxPorts.Items.AddRange(Pic.ComPort.Ports);
            buttonOpenClosePort.Text = Pic.ComPort.Active ? "Stop" : "Start";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Pic.ComPort.Active)
            {
                e.Cancel = true;
                MessageBox.Show("Port still open!", "Error", MessageBoxButtons.OK);
            }
            base.OnClosing(e);
        }

        private void buttonOpenClosePort_Click(object sender, EventArgs e)
        {
            if (Pic.ComPort.Active)
            {
                Pic.ComPort.Stop();
                buttonOpenClosePort.Text = "Start";
            }
            else
            {
                if (comboBoxPorts.SelectedIndex > -1)
                {
                    Pic.ComPort.Start(comboBoxPorts.SelectedItem.ToString());
                    buttonOpenClosePort.Text = "Stop";
                }
            }
        }
    }
}
