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
    public partial class RegisterView : Form
    {
        public RegisterView(RegisterFileMap RegisterMap)
        {
            int sizeOfField = 25;

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScroll = true;
            Size max = SystemInformation.MaxWindowTrackSize;
            this.Size = new System.Drawing.Size(9*(sizeOfField+6), max.Height);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            for(int i = 0; i < 8; i++)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(( sizeOfField + 4) * i+sizeOfField, 4);
                label.Name = "ByteColumn" + i;
                label.Size = new System.Drawing.Size(sizeOfField, sizeOfField - 6);
                label.Text = "0" + i;
                this.Controls.Add(label);
            }
            for(int i = 0; i < 32; i++)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(0, sizeOfField * i + sizeOfField + 3);
                label.Name = "ByteRow" + i;
                label.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
                int content = i * 8;
                label.Text = content.ToString("X2");
                this.Controls.Add(label);
                for(int m = 0; m < 8; m++)
                {
                    TextBox textBox = new TextBox();
                    textBox.Location = new System.Drawing.Point(( sizeOfField + 4) * m + sizeOfField, sizeOfField * i + sizeOfField);
                    textBox.Name = "Byte" + i * 8 + m; 
                    textBox.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
                    textBox.TabIndex = i * 8 + 8 + m;
                    textBox.Text = RegisterMap.getRegisterList[i*8+m].Value.ToString("X2");
                    textBox.TextChanged += new System.EventHandler(textbox_TextChanged); 
                    this.Controls.Add(textBox);
                }
            }
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            return;
        }


    }
}
