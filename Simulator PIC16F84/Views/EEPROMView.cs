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
    public partial class EEPROMView : Form
    {
        EEPROM eeprom;
        int sizeOfField = 25;

        public EEPROMView(EEPROM eeprom)
        {
            InitializeComponent();
            this.eeprom = eeprom;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

            createEepromView();
            AddEventToEeprom();
        }

        private void createEepromView()
        {
            for (int column = 0; column < 8; column++)
            {
                createLabels(sizeOfField, column);
            }
            for (int row = 0; row < 8; row++)
            {
                createColumn0Label(sizeOfField, row);

                for (int textColumn = 0; textColumn < 8; textColumn++)
                {
                    createTextBox(eeprom, sizeOfField, row, textColumn);
                }
            }
        }

        private void createLabels(int sizeOfField, int column)
        {
            Label label = new Label();
            label.Location = new System.Drawing.Point((sizeOfField + 4) * column + sizeOfField, 4);
            label.Name = "ByteColumn" + column;
            label.Size = new System.Drawing.Size(sizeOfField, sizeOfField - 6);
            label.Text = "0" + column;
            this.Controls.Add(label);
        }

        private void createColumn0Label(int sizeOfField, int row)
        {
            Label label = new Label();
            label.Location = new System.Drawing.Point(0, sizeOfField * row + sizeOfField + 3);
            label.Name = "ByteRow" + row;
            label.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
            int content = row * 8;
            label.Text = content.ToString("X2");
            this.Controls.Add(label);
        }

        private void createTextBox(EEPROM eeprom, int sizeOfField, int row, int textColumn)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point((sizeOfField + 4) * textColumn + sizeOfField, sizeOfField * row + sizeOfField);
            textBox.Name = "Byte " + (row * 8 + textColumn);
            textBox.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
            textBox.TabIndex = row * 8 + 8 + textColumn;
            textBox.Text = eeprom.getEEPROMEntry(row * 8 + textColumn).ToString("X2");
            textBox.ReadOnly = true;
            this.Controls.Add(textBox);
        }

        private void AddEventToEeprom()
        {
            eeprom.EepromChanged += new EventHandler(this.EepromContentChanged);
        }

        public void EepromContentChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { EepromContentChanged(sender, e); });
            }
            else
            {
                fillInEepromView();
            }
        }

        public void fillInEepromView()
        {
            for (int index = 0; index < 64; index++)
            {
                var textBoxArray = this.Controls.Find("Byte " + index, true);
                textBoxArray[0].Text = eeprom.getEEPROMEntry(index).ToString("X2");
            }
        }
    }
}
