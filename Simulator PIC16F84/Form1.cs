using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void schliessenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datenblattPIC16C84ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            byte[] PDF = Properties.Resources.Datenblatt_PIC16C84;

            MemoryStream memoryStream = new MemoryStream(PDF);

            FileStream fileStream = new FileStream("Datenblatt_PIC16C84.pdf", FileMode.OpenOrCreate);

            memoryStream.WriteTo(fileStream);
            fileStream.Close();
            memoryStream.Close();

            Process.Start("Datenblatt_PIC16C84.pdf");
        }

    }
}
