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
        private WorkingRegister W;
        private RegisterByte RegByte;
        private int sizeOfField = 15;
        private int marginSmall = 5;
        private int marginTop = 20;

        public RegisterBox(WorkingRegister W)
        {
            this.W = W;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(300,500);
            constructRegisterBox();
        }

        public RegisterBox(RegisterByte RegByte)
        {
            this.RegByte = RegByte;
            constructRegisterBox();
        }

        private void constructRegisterBox()
        {
            InitializeComponent();
            createTextBox();
            createBitCheckboxes();
            registerBoxSettings();
        }

        private void registerBoxSettings()
        {
            this.Height = marginTop * 4 + marginSmall;
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            if (W != null)
                this.Text = "Working Register";
            else
                this.Text = "Register " + RegByte.Index;
        }

        private void createTextBox()
        {
            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(marginSmall, marginSmall);
            textBox.Width = marginSmall*7 + sizeOfField*8;
            textBox.TextChanged += new System.EventHandler(textbox_TextChanged);
            textBox.Name = "Value";
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
            checkbox.Name = "Bit " + (7 - index);
            this.Controls.Add(checkbox);
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                if (W != null)
                {
                    workRegChanged(textBox);
                }
                else
                    fileRegChanged(textBox);
            }
        }

        private void fileRegChanged(TextBox textBox)
        {
            var name = textBox.Name;
            name = name.Substring(5);
            int id;
            if (int.TryParse(name, out id))
            {
                int content;
                try
                {
                    content = Convert.ToInt32(textBox.Text, 16);
                    RegByte.Value = (byte)content;
                }
                catch
                {
                    // TODO
                }
            }
        }

        private void workRegChanged(TextBox textBox)
        {
            int content;
            try
            {
                content = Convert.ToInt32(textBox.Text, 16);
                W.Value.Value = (byte)content;
            }
            catch
            {
                //TODO
            }
        }
    }
}
