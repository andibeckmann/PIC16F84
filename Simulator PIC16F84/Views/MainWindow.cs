using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Text.RegularExpressions;
using Simulator_PIC16F84.Views;

namespace Simulator_PIC16F84
{
    /// <summary>
    /// Main Class which initalize the program
    /// </summary>
    public partial class Main : Form
    {
        RegisterFileMap RegisterMap;
        ProgramMemoryMap UserMemorySpace;
        ProgramMemoryView ProgramView;
        WorkingRegister W;
        StackView StackView;
        RegisterView registerView;
        EEPROMView eepromView;
        Label realTimeCounterView;
        Label runTimeCounterView;
        System.Timers.Timer SimulationTimerFrequency;
        List<int> breakPoints;
        int SimulationFrequency = 10;
        int realTimeCounter;
        public int crystalFrequency = 200;
        int runTimeCounter;
        private System.Windows.Forms.TrackBar frequencySlider;
        private System.Windows.Forms.TextBox textBoxSlider;

        public SerialPortCOM ComPort;
        public SerialCOM ComView;
        public CrystalFrequenceView CrystalFrequenceView;

        /// <summary>
        /// Detailansicht spezieller Register
        /// </summary>
        RegisterBox WReg;
        RegisterBox AReg;
        RegisterBox BReg;
        RegisterBox Status;
        RegisterBox Option;
        RegisterBox Intcon;
        RegisterBox EECON1;

        public Main()
        {
            InitializeComponent();
            InitializeSlider();
            IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            W = new WorkingRegister(-1);
            RegisterMap = new RegisterFileMap(this);

            setUpSpecialRegisterBoxes();
            setupRegisterView();
            setupProgramView();
            setupStackView();
            setupEEPROMView();

            breakPoints = new List<int>();
            setupCrystalFrequency();

            ComPort = new SerialPortCOM(RegisterMap);

            Button MCLR = new Button();
            MCLR.Location = new System.Drawing.Point(1160, 200);
            MCLR.Text = "MCLR Pin";
            MCLR.Click += MCLR_Click;
            this.Controls.Add(MCLR);
        }

        private void MCLR_Click(object sender, System.EventArgs e)
        {
            Reset newReset = new Reset(RegisterMap);
            newReset.resetMCLR();
            RegisterMap.setPowerDownBit();
        }

        private void setUpSpecialRegisterBoxes()
        {
            WReg = setupWorkingRegisterBox(W, new Point(300, 500));
            AReg = setupRegisterBox(RegisterMap.getARegister(false), new Point(515, 500));
            BReg = setupRegisterBox(RegisterMap.getBRegister(false), new Point(730, 500));
            Status = setupRegisterBox(RegisterMap.getStatusRegister(), new Point(945, 500));
            Option = setupRegisterBox(RegisterMap.getOptionRegister(), new Point(300, 618));
            Intcon = setupRegisterBox(RegisterMap.getIntconRegister(), new Point(515, 618));
            EECON1 = setupRegisterBox(RegisterMap.getEECON1(), new Point(730, 618));
        }

        private void setupCrystalFrequency()
        {
            SimulationTimerFrequency = new System.Timers.Timer(10);
            SimulationTimerFrequency.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteCycle);
        }

        private void setupProgramView()
        {
            UserMemorySpace = new ProgramMemoryMap();
            ProgramView = new ProgramMemoryView(UserMemorySpace, this);
            ProgramView.Location = new Point(300, 0);
            ProgramView.MdiParent = this;
            ProgramView.Show();
        }

        private void setupRegisterView()
        {
            registerView = new RegisterView(ref RegisterMap, RegisterMap.mappingArray, WReg, W, AReg, BReg, Status, Option, Intcon, EECON1);
            RegisterMap.Init();
            W.RegisterChanged += new System.EventHandler<int>(registerView.RegisterContentChanged);
            // Set the Parent Form of the Child window.
            registerView.MdiParent = this;
            registerView.Size = new Size { Height = this.Size.Height - 150, Width = 275 };
            registerView.ClearColors();
            // Display the new form.
            registerView.Show();
        }

        private void setupEEPROMView()
        {
            eepromView = new EEPROMView(RegisterMap.getEepromMemory());
            eepromView.MdiParent = this;
            eepromView.Show();
        }

        private RegisterBox setupWorkingRegisterBox(WorkingRegister W, Point location)
        {
            RegisterBox WReg = new RegisterBox(W);
            WReg.MdiParent = this;
            WReg.StartPosition = FormStartPosition.Manual;
            WReg.Location = location;
            WReg.Show();
            return WReg;
        }

        private RegisterBox setupRegisterBox(RegisterByte register, Point location)
        {
            RegisterBox Reg = new RegisterBox(register);
            Reg.MdiParent = this;
            Reg.StartPosition = FormStartPosition.Manual;
            Reg.Location = location;
            Reg.Show();
            return Reg;
        }

        private void InitializeSlider()
        {
            this.textBoxSlider = new System.Windows.Forms.TextBox();
            this.frequencySlider = new System.Windows.Forms.TrackBar();
            this.realTimeCounterView = new System.Windows.Forms.Label();
            this.runTimeCounterView = new System.Windows.Forms.Label();

            // TextBox for TrackBar.Value update.
            this.textBoxSlider.Location = new System.Drawing.Point(1180, 45 + 25);
            this.textBoxSlider.Size = new System.Drawing.Size(48, 20);
            this.textBoxSlider.Text = Frequency + " ms";
            this.textBoxSlider.TextChanged += new System.EventHandler(this.textBoxSlider_Changed);

            // Setup RealTimeCounter
            realTimeCounterView.Location = new System.Drawing.Point(1180, 45 + 25 + 20);
            realTimeCounterView.Size = new System.Drawing.Size(60, 20);
            realTimeCounterView.TextAlign = ContentAlignment.MiddleRight;
            UpdateRealTimeCounter(realTimeCounter);

            // Setup RunTimeCounter
            runTimeCounterView.Location = new System.Drawing.Point(1180, 120);
            runTimeCounterView.Size = new System.Drawing.Size(100, 20);
            runTimeCounterView.TextAlign = ContentAlignment.MiddleRight;
            UpdateRunTimeCounter(runTimeCounter);

            // Set up how the form should be displayed and add the controls to the form.
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.textBoxSlider, this.frequencySlider, this.realTimeCounterView, this.runTimeCounterView });

            // Set up the TrackBar.
            this.frequencySlider.Location = new System.Drawing.Point(1180, 25);
            this.frequencySlider.Size = new System.Drawing.Size(224, 45);
            this.frequencySlider.Scroll += new System.EventHandler(this.frequencySlider_Scroll);

            // The Maximum property sets the value of the track bar when
            // the slider is all the way to the right.
            frequencySlider.Maximum = 1000;

            // The Minimum property sets the value of the track bar when
            // the slider is all the way to the left.
            frequencySlider.Minimum = 10;

            // The TickFrequency property establishes how many positions
            // are between each tick-mark.
            frequencySlider.TickFrequency = 10;

            // The LargeChange property sets how many positions to move
            // if the bar is clicked on either side of the slider.
            frequencySlider.LargeChange = 5;

            // The SmallChange property sets how many positions to move
            // if the keyboard arrows are used to move the slider.
            frequencySlider.SmallChange = 2;

        }

        private void UpdateRealTimeCounter(int newValue)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { UpdateRealTimeCounter(newValue); });
            }
            else
            {
                var seconds = newValue / 1000;
                var milliseconds = newValue % 1000;
                realTimeCounterView.Text = seconds + "." + milliseconds + " s";
            }
        }

        private void UpdateRunTimeCounter(int newValue)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { UpdateRunTimeCounter(newValue); });
            }
            else
            {
                var milliseconds = newValue / 1000;
                var nanoseconds = newValue % 1000;
                runTimeCounterView.Text = milliseconds + "." + nanoseconds + " ms";
            }
        }

        private void frequencySlider_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBoxSlider.Text = frequencySlider.Value + " ms";
            Frequency = frequencySlider.Value;
        }

        public int Frequency
        {
            get
            {
                return SimulationFrequency;
            }
            set
            {
                SimulationFrequency = value;
                SimulationTimerFrequency.Interval = SimulationFrequency;
            }
        }

        private void textBoxSlider_Changed(object sender, System.EventArgs e)
        {
            var resultString = Regex.Match(this.textBoxSlider.Text, @"\d+").Value;
            if (this.textBoxSlider.Text != "" && int.Parse(resultString) <= frequencySlider.Maximum && int.Parse(resultString) >= frequencySlider.Minimum)
            {
                Frequency = int.Parse(resultString);
                frequencySlider.Value = Frequency;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TODO ups... gut, dass hier rein gar nichts steht :D
        }

        private void schliessenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void datenblattPIC16F84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPDFResource(Properties.Resources.Datenblatt_PIC16F84, "Datenblatt_PIC16F84");
        }

        private static void OpenPDFResource(byte[] PDFResource, string fileName)
        {


            string tempPath = Path.GetTempPath();
            tempPath += "/" + fileName + ".pdf";

            if (!File.Exists(tempPath))
            {
                byte[] PDF = PDFResource;

                MemoryStream memoryStream = new MemoryStream(PDF);

                FileStream fileStream = new FileStream(tempPath, FileMode.OpenOrCreate);

                memoryStream.WriteTo(fileStream);
                fileStream.Close();
                memoryStream.Close();
            }

            Process.Start(tempPath);
        }

        private void projektToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPDFResource(Properties.Resources.Projekt_Simulator, "Projekt_Simulator");
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {

        }

        private void programmLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SimulationTimerFrequency.Stop();
            OpenFileDialog programmLaden = new OpenFileDialog();

            //TODO: Programm Laden Auswahl wieder einfügen
            programmLaden.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Programme";
            programmLaden.Filter = "lst files (*.lst)|*.lst";
            programmLaden.FilterIndex = 1;
            programmLaden.RestoreDirectory = false;

            if (programmLaden.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(programmLaden.FileName);
                    string fileContent = sr.ReadToEnd();
                    sr.Close();
                    resetSimulation();
                    ProgramView.loadProgram(fileContent);
                    UserMemorySpace = ProgramView.getBinaryCode();
                    RegisterMap.PC.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file\n" + ex.Message);
                }
            }

            /// Direkter Aufruf zum Testen
            //String testPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Programme\\";

            //System.IO.StreamReader sr = new System.IO.StreamReader(testPath + "SimTest1.lst");
            //string fileContent = sr.ReadToEnd();
            //sr.Close();
            //ProgramView.loadProgram(fileContent);
            //UserMemorySpace = ProgramView.getBinaryCode();
            //PC.Clear();
        }

        private void ExecuteCycle(object source, ElapsedEventArgs e)
        {
                var index = FindRowForPC(RegisterMap.PC.Counter.Address);
                if (breakPoints.Contains(index))
                {
                    SimulationTimerFrequency.Stop();
                    return;
                }
                ExecuteSingleCycle(index);
        }

        private void ExecuteSingleCycle(int index)
        {
            this.registerView.ClearColors();
            if (!RegisterMap.isInPowerDownMode())
            {
                UserMemorySpace.ProgramMemory[RegisterMap.PC.Counter.Address].DecodeInstruction(RegisterMap, W, RegisterMap.PC, RegisterMap.getStack());
                RegisterMap.PC.InkrementPC();
                SetSelection(index);
                realTimeCounter += SimulationFrequency;
                UpdateRealTimeCounter(realTimeCounter);
                UpdateRunTimeCounter(runTimeCounter);
            }
            checkForTimeOut();
            RegisterMap.checkForInterrupt();
            RegisterMap.checkEEPROMFunctionality();
            RegisterMap.checkWatchdogTimer();
        }

        private void checkForTimeOut()
        {
            if (!RegisterMap.isTimeOutBitSet())
            {
                WatchDogTimerReset();
                RegisterMap.setTimeOutBit();
            }
        }

        private void SetSelection(int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { SetSelection(index); });
            }
            else
            {
                try
                {
                    this.ProgramView.dataGridView1.CurrentCell = this.ProgramView.dataGridView1[0, index];
                    this.ProgramView.dataGridView1.ClearSelection();
                    this.ProgramView.dataGridView1.Rows[index].Selected = true;
                }
                catch
                {
                    return;
                }
            }
        }

        private int FindRowForPC(int counter)
        {
            int rowIndex = 0;
            foreach (DataGridViewRow row in this.ProgramView.dataGridView1.Rows)
            {
                if (row.Cells[1].Value == null)
                {
                    continue;
                }
                if (row.Cells[1].Value.ToString().Equals(counter.ToString("X4")))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            return rowIndex;
        }

        public void HandleBreakpoint(object sender, int index)
        {
            if (breakPoints.Contains(index))
            {
                breakPoints.Remove(index);
            }
            else
            {
                breakPoints.Add(index);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SimulationTimerFrequency.Start();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resetSimulation();
        }

        private void WatchDogTimerReset()
        {
            Reset newReset = new Reset(RegisterMap);
            newReset.resetWDT();
            RegisterMap.setPowerDownBit();
        }

        private void resetSimulation()
        {
            SimulationTimerFrequency.Stop();
            RegisterMap.ClearWatchdogTimer();
            RegisterMap.PC.Clear();
            SetSelection(0);
            RegisterMap.ClearRegister();
            RegisterMap.Init();
            registerView.ClearColors();
            RegisterMap.getStack().ClearStack();
            W.ClearRegister();
            RegisterMap.getEepromMemory().clearEEPROM();
            realTimeCounter = 0;
            UpdateRealTimeCounter(realTimeCounter);
            runTimeCounter = 0;
            UpdateRunTimeCounter(runTimeCounter);
        }

        private void unterbrechenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SimulationTimerFrequency.Stop();
        }

        private void einzelschrittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = FindRowForPC(RegisterMap.PC.Counter.Address);
            ExecuteSingleCycle(index);
        }

        private void programMemoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupProgramView();
        }

        private void registerFileMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupRegisterView();
        }

        private void stackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupStackView();
        }

        private void setupStackView()
        {
            StackView = new StackView(RegisterMap.getStack());
            StackView.MdiParent = this;
            StackView.Show();
        }

        private void workingRegisterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WReg = setupWorkingRegisterBox(W, new Point(300, 500));
        }

        private void aRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AReg = setupRegisterBox(RegisterMap.getARegister(false), new Point(515, 500));
        }

        private void bRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BReg = setupRegisterBox(RegisterMap.getBRegister(false), new Point(730, 500));
        }

        private void optionRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Option = setupRegisterBox(RegisterMap.getOptionRegister(), new Point(515, 610));
        }

        private void intconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Intcon = setupRegisterBox(RegisterMap.getIntconRegister(), new Point(730, 610));
        }

        private void statusRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status = setupRegisterBox(RegisterMap.getStatusRegister(), new Point(300, 610));
        }

        private void eECON1RegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EECON1 = setupRegisterBox(RegisterMap.getEECON1(), new Point(300, 720));
        }

        private void eEPROMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupEEPROMView();
        }

        private void serialPortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ComView == null)
            {
                ComView = new SerialCOM(this);
            }
            ComView.Show();
            ComView.Focus();
        }


        internal void incrementRunTimeCounter()
        {
            runTimeCounter += crystalFrequency;
        }

        private void crystalFrequenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CrystalFrequenceView == null)
            {
                CrystalFrequenceView = new CrystalFrequenceView(this, crystalFrequency);
            }
            CrystalFrequenceView.Show();
            CrystalFrequenceView.Focus();
        }
    }
}
