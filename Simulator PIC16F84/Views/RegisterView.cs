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
        RegisterBox workingRegisterBox;
        WorkingRegister W;
        RegisterBox ARegRegisterBox;
        RegisterBox BRegRegisterBox;
        RegisterBox StatusRegisterBox;
        RegisterBox OptionRegisterBox;
        RegisterBox IntconRegisterBox;
        RegisterBox Eecon1RegisterBox;

        public RegisterView(ref RegisterFileMap RegisterMap, int[] mappingArray, RegisterBox registerBox, WorkingRegister W, RegisterBox AReg, RegisterBox BReg, RegisterBox Status, RegisterBox Option, RegisterBox Intcon, RegisterBox Eecon1)
        {
            int sizeOfField = 25;

            this.registerMap = RegisterMap;
            this.workingRegisterBox = registerBox;
            this.W = W;
            this.ARegRegisterBox = AReg;
            this.BRegRegisterBox = BReg;
            this.StatusRegisterBox = Status;
            this.OptionRegisterBox = Option;
            this.IntconRegisterBox = Intcon;
            this.Eecon1RegisterBox = Eecon1;

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScroll = true;
            Size max = SystemInformation.MaxWindowTrackSize;
            this.Size = new System.Drawing.Size(9 * (sizeOfField + 6), max.Height);
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.mappingArray = mappingArray;
            AddEventToRegister();
            createRegisterPattern(RegisterMap, sizeOfField);
            initSpecialRegisters();
        }

        private void createRegisterPattern(RegisterFileMap RegisterMap, int sizeOfField)
        {
            for (int column = 0; column < 8; column++)
            {
                createLabels(sizeOfField, column);
            }
            for (int row = 0; row < 32; row++)
            {
                createColumn0Label(sizeOfField, row);

                for (int textColumn = 0; textColumn < 8; textColumn++)
                {
                    createTextBox(RegisterMap, sizeOfField, row, textColumn);
                }
            }
        }

        private void createColumn0Label(int sizeOfField, int i)
        {
            Label label = new Label();
            label.Location = new System.Drawing.Point(0, sizeOfField * i + sizeOfField + 3);
            label.Name = "ByteRow" + i;
            label.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
            int content = i * 8;
            label.Text = content.ToString("X2");
            this.Controls.Add(label);
        }

        private void createTextBox(RegisterFileMap RegisterMap, int sizeOfField, int i, int m)
        {
            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point((sizeOfField + 4) * m + sizeOfField, sizeOfField * i + sizeOfField);
            textBox.Name = "Byte " + (i * 8 + m);
            textBox.Size = new System.Drawing.Size(sizeOfField, sizeOfField);
            textBox.TabIndex = i * 8 + 8 + m;
            textBox.Text = RegisterMap.getRegister(i * 8 + m,false).Value.ToString("X2");
            textBox.TextChanged += new System.EventHandler(textbox_TextChanged);
            this.Controls.Add(textBox);
        }

        private void createLabels(int sizeOfField, int i)
        {
            Label label = new Label();
            label.Location = new System.Drawing.Point((sizeOfField + 4) * i + sizeOfField, 4);
            label.Name = "ByteColumn" + i;
            label.Size = new System.Drawing.Size(sizeOfField, sizeOfField - 6);
            label.Text = "0" + i;
            this.Controls.Add(label);
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
                        registerMap.getRegister(id,true).Value = (byte)content;
                    }
                    catch
                    {
                        // TODO
                    }
                }
            }
        }

        public void RegisterContentChanged(object sender, int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { RegisterContentChanged(sender, index); });
            }
            else
            {
                FillInRegBytes(index);
                CheckSpecialRegisters(index);
            }
        }

        /// <summary>
        /// Check if Bit is set in Byte
        /// </summary>
        /// <param name="byteValue">Byte</param>
        /// <param name="bit">Bit</param>
        /// <returns>result</returns>
        private bool IsBitSet(int byteValue, int bit)
        {
            return (byteValue & (1 << bit)) != 0;
        }

        private void CheckSpecialRegisters(int index)
        {
            if (index == -1)
                checkRegister(workingRegisterBox, W);
            else if (index == 0x01)
                registerMap.checkTimerRegister();
            else if (index == 0x05)
            {
                checkRegister(ARegRegisterBox, registerMap.getARegister(false));
                if ( registerMap.timer0InCounterMode())
                    registerMap.checkCountingConditions();
            }
            else if (index == 0x06)
            {
                registerMap.checkForIntInterrupt();
                registerMap.checkForPortBInterrupt();
                checkRegister(BRegRegisterBox, registerMap.getBRegister(false));
            }
            else if (index == 0x03)
                checkRegister(StatusRegisterBox, registerMap.getStatusRegister());
            else if (index == 0x0B)
                checkRegister(IntconRegisterBox, registerMap.getIntconRegister());
            else if (index == 0x81)
            {
                checkRegister(OptionRegisterBox, registerMap.getOptionRegister());
                registerMap.checkOptionRegisterSettings();
            }
            else if (index == 0x85)
            {
                ARegRegisterBox.updateIOView(registerMap.getTRISA());
            }
            else if (index == 0x86)
            {
                BRegRegisterBox.updateIOView(registerMap.getTRISB());
            }
            else if (index == 0x88)
            {
                checkRegister(Eecon1RegisterBox, registerMap.getEECON1());
            }
        }

        private void initSpecialRegisters()
        {
            checkRegister(workingRegisterBox, W);
            checkRegister(ARegRegisterBox, registerMap.getARegister(false));
            checkRegister(BRegRegisterBox, registerMap.getBRegister(false));
            checkRegister(StatusRegisterBox, registerMap.getStatusRegister());
            checkRegister(IntconRegisterBox, registerMap.getIntconRegister());
            checkRegister(OptionRegisterBox, registerMap.getOptionRegister());

            registerMap.checkOptionRegisterSettings();
        }

        private void checkRegister(RegisterBox box, RegisterByte Reg)
        {
            var textBoxArray = box.Controls.Find("Value", true);
            textBoxArray[0].Text = Reg.Value.ToString("X2");
            updateCheckBoxes(box, Reg);
        }

        private void updateCheckBoxes(RegisterBox box, RegisterByte Reg)
        {
            for (int i = 0; i < 8; i++)
            {
                var checkBoxArray = box.Controls.Find("Bit " + i, true);
                if (Reg.isBitSet(i))
                {
                    var checkBox = ((CheckBox)checkBoxArray[0]).Checked = true;
                }
                else
                {
                    var checkBox = ((CheckBox)checkBoxArray[0]).Checked = false;
                }
            }
        }

        private void FillInRegBytes(int index)
        {
            for (int i = 0; i < mappingArray.Length; i++)
            {
                if (mappingArray[i] == index)
                {
                    var textBoxArray = this.Controls.Find("Byte " + i, true);
                    if (textBoxArray.Length > 0)
                    {
                        ///Korrektes Belegen der Registereinträge (getRegisterList liefert Register direkt zurück,
                        ///allerdings sind einige spezielle Register (Status, PCL, etc.) in beiden Bänken miteinander
                        ///verbunden. Die getRegister-Methode greift auf das dafür erstellte Mappingarray zu und
                        ///berücksichtigt diese speziellen Register (allerdings auch eine evtl. indirekte Adressierung
                        ///via Bankumschaltung, deshalb wird das ignoreBankSelection-Bit gesetzt.
                        textBoxArray[0].Text = registerMap.getRegister(i, false).Value.ToString("X2");
                        textBoxArray[0].BackColor = Color.Red;
                    }
                }
            }
        }

        private void AddEventToRegister()
        {
            for (int i = 0; i < registerMap.getRegisterList.Length; i++)
            {
                registerMap.getRegister(i, false).RegisterChanged += new EventHandler<int>(this.RegisterContentChanged);
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
                for (int i = 0; i < this.Controls.Count; i++)
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
