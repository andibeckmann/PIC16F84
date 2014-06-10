using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Simulator_PIC16F84
{
    public partial class RegisterBox : Form
    {
        private WorkingRegister W;
        private RegisterByte RegByte;
        private int sizeOfField = 19;
        private int marginSmall = 5;
        private int marginTop = 20;

        public RegisterBox(WorkingRegister W)
        {
            this.W = W;
            this.Location = new Point(300, 500);
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
            this.Height = marginTop * 4 + marginSmall + sizeOfField;
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            if (W != null)
            {
                this.Text = "WORKING Register";
                this.Height -= sizeOfField;
            }
            else if (RegByte.Index == 3)
                this.Text = "STATUS Register";
            else if (RegByte.Index == 5)
                this.Text = "PORT A Register";
            else if (RegByte.Index == 6)
                this.Text = "PORT B Register";
            else if (RegByte.Index == 0x81)
                this.Text = "OPTION Register";
            else if (RegByte.Index == 0x0B)
                this.Text = "INTCON Register";
            else
                this.Text = "Register " + RegByte.Index;
        }

        private void createTextBox()
        {
            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(marginSmall, marginSmall);
            textBox.Width = marginSmall * 7 + sizeOfField * 8;
            textBox.TextChanged += new System.EventHandler(textbox_TextChanged);
            textBox.Name = "Value";
            textBox.TextAlign = HorizontalAlignment.Right;
            this.Controls.Add(textBox);
        }

        private void createBitCheckboxes()
        {
            for (int index = 0; index < 8; index++)
            {
                createCheckBox(index);
            }
        }

        private void createCheckBox(int index)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.Location = new System.Drawing.Point((sizeOfField + marginSmall) * index + marginSmall, marginTop + marginSmall);
            checkbox.Width = sizeOfField;
            checkbox.Name = "Bit " + (7 - index);
            checkbox.CheckedChanged += new System.EventHandler(CheckBoxChanged);

            if (RegByte != null)
                createLabels(index);
            this.Controls.Add(checkbox);
        }

        private void createLabels(int index)
        {
            Label label = new Label();
            label.Location = new System.Drawing.Point((sizeOfField + marginSmall) * index + 3, marginTop * 2 + marginSmall);
            label.Width = sizeOfField;
            label.Font = new Font("Arial", 5);
            if (RegByte.Index == 3)
                getStatusRegLabels(7 - index, label);
            else if (RegByte.Index == 5)
                getPortARegLabels(7 - index, label);
            else if (RegByte.Index == 6)
                getPortBRegLabels(7 - index, label);
            else if (RegByte.Index == 0x81)
                getOptionRegLabels(7 - index, label);
            else if (RegByte.Index == 0x0B)
                getIntconRegLabels(7 - index, label);
            this.Controls.Add(label);
        }

        /// <summary>
        /// Sets an explanatory text for a Status Register Label
        /// </summary>
        /// <param name="index">Index for Bit-Number of the Label</param>
        /// <param name="label">Label which will receive the text</param>
        private static void getStatusRegLabels(int index, Label label)
        {
            if (index == 0)
                label.Text = "C";
            else if (index == 1)
                label.Text = "DC";
            else if (index == 2)
                label.Text = "Z";
            else if (index == 3)
                label.Text = "PD";
            else if (index == 4)
                label.Text = "TO";
            else if (index == 5)
                label.Text = "RP0";
            else
                label.Text = "  -";
        }

        /// <summary>
        /// Sets an explanatory text for a PortA Register Label
        /// </summary>
        /// <param name="index">Index for Bit-Number of the Label</param>
        /// <param name="label">Label which will receive the text</param>
        private static void getPortARegLabels(int index, Label label)
        {
            if (index == 0)
                label.Text = "RA0";
            else if (index == 1)
                label.Text = "RA1";
            else if (index == 2)
                label.Text = "RA2";
            else if (index == 3)
                label.Text = "RA3";
            else if (index == 4)
                label.Text = "RA4/T0CKI";
            else
                label.Text = "  -";
        }

        /// <summary>
        /// Sets an explanatory text for a PortB Register Label
        /// </summary>
        /// <param name="index">Index for Bit-Number of the Label</param>
        /// <param name="label">Label which will receive the text</param>
        private static void getPortBRegLabels(int index, Label label)
        {
            if (index == 0)
                label.Text = "RB0/INT";
            else if (index == 1)
                label.Text = "RB1";
            else if (index == 2)
                label.Text = "RB2";
            else if (index == 3)
                label.Text = "RB3";
            else if (index == 4)
                label.Text = "RB4";
            else if (index == 5)
                label.Text = "RB5";
            else if (index == 6)
                label.Text = "RB6";
            else
                label.Text = "RB7";
        }

        /// <summary>
        /// Sets an explanatory text for an Option Register Label
        /// </summary>
        /// <param name="index">Index for Bit-Number of the Label</param>
        /// <param name="label">Label which will receive the text</param>
        private static void getOptionRegLabels(int index, Label label)
        {
            if (index < 3)
                label.Text = "PS" + index;
            else if (index == 3)
                label.Text = "PSA";
            else if (index == 4)
                label.Text = "T0SE";
            else if (index == 5)
                label.Text = "T0CS";
            else if (index == 6)
                label.Text = "INTEDG";
            else
                label.Text = "RBPU";
        }

        /// <summary>
        /// Sets an explanatory text for an Interrupt Control Register Label
        /// </summary>
        /// <param name="index">Index for Bit-Number of the Label</param>
        /// <param name="label">Label which will receive the text</param>
        private static void getIntconRegLabels(int index, Label label)
        {
            if (index == 0)
                label.Text = "RBIF";
            else if (index == 1)
                label.Text = "INTF";
            else if (index == 2)
                label.Text = "T0IF";
            else if (index == 3)
                label.Text = "RBIE";
            else if (index == 4)
                label.Text = "INTE";
            else if (index == 5)
                label.Text = "T0IE";
            else if (index == 6)
                label.Text = "EEIE";
            else
                label.Text = "GIE";
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

        private void workRegChanged(TextBox textBox)
        {
            int content;
            try
            {
                content = Convert.ToInt32(textBox.Text, 16);
                W.Value = (byte)content;
            }
            catch
            {
                //TODO
            }
        }

        public void CheckBoxChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox != null)
            {
                var resultString = Regex.Match(checkBox.Name, @"\d+").Value;
                var bit = Int32.Parse(resultString);
                if (checkBox.Checked == true)
                {
                    if (W != null)
                    {
                        W.Value = SetBit(W.Value, bit);
                    }
                    else
                    {
                        RegByte.Value = SetBit(RegByte.Value, bit);
                    }
                }
                else
                {
                    if(W != null)
                    {
                        W.Value = ClearBit(W.Value, bit);
                    }
                    else
                    {
                        RegByte.Value = ClearBit(RegByte.Value, bit);
                    }
                }
            }
        }
        /// <summary>
        /// Setzt ein bestimmtes Bit in einem Byte.
        /// </summary>
        /// <param name="b">Byte, welches bearbeitet werden soll.</param>
        /// <param name="BitNumber">Das zu setzende Bit (0 bis 7).</param>
        /// <returns>Ergebnis - Byte</returns>
        public static byte SetBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b | (byte)(0x01 << BitNumber));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }

        /// <summary>
        /// Löscht ein bestimmtes Bit in einem Byte.
        /// </summary>
        /// <param name="b">Byte, welches bearbeitet werden soll.</param>
        /// <param name="BitNumber">Das zu löschende Bit (0 bis 7).</param>
        /// <returns>Ergebnis - Byte</returns>
        public static byte ClearBit(byte b, int BitNumber)
        {
            //Kleine Fehlerbehandlung
            if (BitNumber < 8 && BitNumber > -1)
            {
                return (byte)(b & (byte)(~(0x01 << BitNumber)));
            }
            else
            {
                throw new InvalidOperationException(
                "Der Wert für BitNumber " + BitNumber.ToString() + " war nicht im zulässigen Bereich! (BitNumber = (min)0 - (max)7)");
            }
        }
    }
}
