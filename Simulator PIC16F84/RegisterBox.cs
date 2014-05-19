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
    public partial class RegisterBox : Form
    {
        private int sizeOfField = 15;
        private int marginSmall = 5;
        private int marginTop = 20;

        public RegisterBox()
        {
            InitializeComponent();
            createTextBox();
            createBitCheckboxes();
            this.Height = marginTop*4+marginSmall;
            this.Text = "Working Register";
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void createTextBox()
        {
            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(marginSmall, marginSmall);
            textBox.Width = marginSmall*7 + sizeOfField*8;
            this.Controls.Add(textBox);
        }

        private void createBitCheckboxes()
        {
            for (int index = 0; index < 8; index++)
                createCheckBox(index);
        }

        private void createCheckBox(int index)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.Location = new System.Drawing.Point((sizeOfField + marginSmall) * index + marginSmall, marginTop + marginSmall);
            checkbox.Width = sizeOfField;
            this.Controls.Add(checkbox);
        }
    }
}
