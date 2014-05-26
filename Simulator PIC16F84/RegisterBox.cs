﻿using System;
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
        private int sizeOfField = 15;
        private int marginSmall = 5;
        private int marginTop = 20;

        public RegisterBox(WorkingRegister W)
        {
            this.W = W;
            this.StartPosition = FormStartPosition.Manual;
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
            this.Height = marginTop * 4 + marginSmall;
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            if (W != null)
                this.Text = "WORKING Register";
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
