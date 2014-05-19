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

        public StackView( ref Stack Stack)
        {
            this.Stack = Stack;

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            for (int i = 0; i < 8; i++)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(5, i*(sizeOfField+2) + 5);
                label.Name = "Level " + i;
                label.Size = new System.Drawing.Size(sizeOfField * 5, 15);
                label.Text = "Level " + i;
                this.Controls.Add(label);
            }
        }

        private void StackContentChanged(object sender, int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { StackContentChanged(sender, index); });
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    
                }
            }
        }

        private void AddEventToStack()
        {
//            Stack.StackChanged += new EventHandler<int>(this.StackContentChanged);
        }
    }
}
