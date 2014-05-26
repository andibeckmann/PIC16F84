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
    public partial class StackView : Form
    {
        private Stack Stack;
        private int sizeOfField = 20;
        private int marginSmall = 5;

        public StackView( Stack Stack)
        {
            this.Stack = Stack;

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            createStackLabel();
            createStackTextboxes();
            AddEventToStack();
            fillInStack();
        }

        private void createStackTextboxes()
        {
            for (int index = 0; index < 8; index++)
            {
                TextBox textBox = new TextBox();
                textBox.Location = new System.Drawing.Point((marginSmall + sizeOfField )* 2, marginSmall + index * (sizeOfField + marginSmall ));
                textBox.Size = new System.Drawing.Size(sizeOfField * 4, sizeOfField);
                textBox.Name = "Stack Level " + index;
                textBox.ReadOnly = true;
                textBox.TextAlign = HorizontalAlignment.Right;
                this.Controls.Add(textBox);
            }
        }

        private void createStackLabel()
        {
            for (int index = 0; index < 8; index++)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(marginSmall, marginSmall*2 + index * (sizeOfField + marginSmall));
                label.Name = "Level " + index;
                label.Size = new System.Drawing.Size(sizeOfField * 2 + marginSmall, sizeOfField);
                label.Text = "Level " + index;
                this.Controls.Add(label);
            }
        }

        private void AddEventToStack()
        {
                Stack.Value.StackChanged += new EventHandler<int>(this.StackContentChanged);
        }

        public void StackContentChanged(object sender, int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { StackContentChanged(sender, index); });
            }
            else
            {
                fillInStack();
            }
        }

        public void fillInStack()
        {
            for (int index = 0; index < 8; index++)
            {
                var textBoxArray = this.Controls.Find("Stack Level " + index, true);
                textBoxArray[0].Text = Stack.getStackValues(index).Value.ToString("X2");
            }
        }
    }
}
