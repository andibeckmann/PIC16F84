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
        RegisterFileMap registerMap;
        int[] mappingArray;

        public RegisterView(ref RegisterFileMap RegisterMap, int[] mappingArray)
        {
            this.registerMap = RegisterMap;

            int sizeOfField = 25;

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScroll = true;
            Size max = SystemInformation.MaxWindowTrackSize;
            this.Size = new System.Drawing.Size(9*(sizeOfField+6), max.Height);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            this.mappingArray = mappingArray;
            AddEventToRegister();

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
                    textBox.Name = "Byte " + (i * 8 + m); 
                    textBox.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
                    textBox.TabIndex = i * 8 + 8 + m;
                    textBox.Text = RegisterMap.getRegister(i*8+m).Value.ToString("X2");
                    textBox.TextChanged += new System.EventHandler(textbox_TextChanged); 
                    this.Controls.Add(textBox);
                }
            }
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
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
                        registerMap.getRegister(id).Value = (byte)content;
                    }
                    catch
                    {
                        // TODO
                    }
                }
            }
        }

        private void RegisterContentChanged(object sender, int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { RegisterContentChanged(sender, index); });
            }
            else
            {
                for (int i = 0; i < mappingArray.Length; i++)
                {
                    if (mappingArray[i] == index)
                    {
                        var textBoxArray = this.Controls.Find("Byte " + i, true);
                        textBoxArray[0].Text = registerMap.getRegister(i).Value.ToString("X2");
                        textBoxArray[0].BackColor = Color.Red;
                    }
                }
            }
        }

        private void AddEventToRegister()
        {
            for(int i = 0 ; i < registerMap.getRegisterList.Length; i++ )
            {
                registerMap.getRegister(i).RegisterChanged += new EventHandler<int>(this.RegisterContentChanged);
            }
        }



        public void ClearColors()
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { ClearColors(); });
            }
            else
            {
                for (int i = 0; i < this.Controls.Count; i++ )
                {
                    if (this.Controls[i].GetType() == typeof(TextBox))
                    {
                        this.Controls[i].BackColor = Color.White;
                    }
                }
            }
        }
    }
}
