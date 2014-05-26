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
        ProgramCounter PC;
        Stack Stack;
        StackView StackView;
        WatchdogTimer WDT;
        Prescaler Prescaler;
        RegisterView registerView;
        System.Timers.Timer crystalFrequency;
        List<int> breakPoints;
        int frequency = 10;
        private System.Windows.Forms.TrackBar frequencySlider;
        private System.Windows.Forms.TextBox textBoxSlider;

        /// <summary>
        /// Detailansicht spezieller Register
        /// </summary>
        RegisterBox WBox;
        RegisterBox AReg;
        RegisterBox BReg;
        RegisterBox Status;
        RegisterBox Option;
        RegisterBox Intcon;

        public Main()
        {
            InitializeComponent();
            InitializeSlider();
            IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            W = new WorkingRegister(-1);

            RegisterMap = new RegisterFileMap(PC);

            setupWorkingRegisterBox();
            setupRegisterBoxA();
            setupRegisterBoxB();
            setupStatusBox();
            setupOptionRegister();
            setupIntconRegister();

            setupRegisterView();
            setupProgramView();

            PC = new ProgramCounter(RegisterMap);
            setupStack();
            //Watchdogtimer
            WDT = new WatchdogTimer();
            Prescaler = new Prescaler();
            breakPoints = new List<int>();
            setupCrystalFrequency();
        }

        private void setupCrystalFrequency()
        {
            crystalFrequency = new System.Timers.Timer(10);
            crystalFrequency.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteCycle);
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
            registerView = new RegisterView(ref RegisterMap, RegisterMap.mappingArray, WBox, W, AReg, BReg, Status, Option, Intcon);
            RegisterMap.Init();
            W.RegisterChanged += new System.EventHandler<int>(registerView.RegisterContentChanged);
            // Set the Parent Form of the Child window.
            registerView.MdiParent = this;
            registerView.Size = new Size { Height = this.Size.Height - 150, Width = 275 };
            registerView.ClearColors();
            // Display the new form.
            registerView.Show();
        }

        private void setupStatusBox()
        {
            Status = new RegisterBox(RegisterMap.getStatusRegister());
            Status.MdiParent = this;
            Status.StartPosition = FormStartPosition.Manual;
            Status.Location = new Point(300, 600);
            Status.Show();
        }

        private void setupRegisterBoxB()
        {
            ///B-Register View
            BReg = new RegisterBox(RegisterMap.getBRegister());
            BReg.MdiParent = this;
            BReg.StartPosition = FormStartPosition.Manual;
            BReg.Location = new Point(750, 500);
            BReg.Show();
        }

        private void setupRegisterBoxA()
        {
            ///A-Register View
            AReg = new RegisterBox(RegisterMap.getARegister());
            AReg.MdiParent = this;
            AReg.StartPosition = FormStartPosition.Manual;
            AReg.Location = new Point(525, 500);
            AReg.Show();
        }

        private void setupWorkingRegisterBox()
        {
            /// Working Register View
            WBox = new RegisterBox(W);
            WBox.MdiParent = this;
            WBox.Show();
        }

        private void setupOptionRegister()
        {
            Option = new RegisterBox(RegisterMap.getOptionRegister());
            Option.MdiParent = this;
            Option.StartPosition = FormStartPosition.Manual;
            Option.Location = new Point(525, 600);
            Option.Show();
        }

        private void setupIntconRegister()
        {
            Intcon = new RegisterBox(RegisterMap.getIntconRegister());
            Intcon.MdiParent = this;
            Intcon.StartPosition = FormStartPosition.Manual;
            Intcon.Location = new Point(750, 600);
            Intcon.Show();
        }

        private void InitializeSlider()
        {
            this.textBoxSlider = new System.Windows.Forms.TextBox();
            this.frequencySlider = new System.Windows.Forms.TrackBar();

            // TextBox for TrackBar.Value update.
            this.textBoxSlider.Location = new System.Drawing.Point(1180, 45+25);
            this.textBoxSlider.Size = new System.Drawing.Size(48, 20);
            this.textBoxSlider.Text = Frequency + " ms";
            this.textBoxSlider.TextChanged += new System.EventHandler(this.textBoxSlider_Changed);

            // Set up how the form should be displayed and add the controls to the form.
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.textBoxSlider, this.frequencySlider });

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
                return frequency;
            }
            set
            {
                frequency = value;
                crystalFrequency.Interval = frequency;
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
                    ProgramView.loadProgram(fileContent);
                    UserMemorySpace = ProgramView.getBinaryCode();
                    PC.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file" + ex.Message);
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
            var index = FindRowForPC(PC.Counter.Value);
            if(breakPoints.Contains(index))
            {
                crystalFrequency.Stop();
                return;
            }
            ExecuteSingleCycle(index);
        }

        private void ExecuteSingleCycle(int index)
        {
            this.registerView.ClearColors();
            UserMemorySpace.ProgramMemory[PC.Counter.Value].DecodeInstruction(RegisterMap, W, PC, Stack, WDT, Prescaler);
            PC.InkrementPC();
            SetSelection(index);
        }

        private void SetSelection(int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { SetSelection(index); });
            }
            else
            {
                this.ProgramView.dataGridView1.CurrentCell = this.ProgramView.dataGridView1[0, index];
                this.ProgramView.dataGridView1.ClearSelection();
                this.ProgramView.dataGridView1.Rows[index].Selected = true;
            }
        }

        private int FindRowForPC(int counter)
        {
            int rowIndex = 0;
            foreach (DataGridViewRow row in this.ProgramView.dataGridView1.Rows)
            {
                if(row.Cells[1].Value == null)
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
            if(breakPoints.Contains(index))
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
            var index = FindRowForPC(PC.Counter.Value);
            ExecuteSingleCycle(index);
            crystalFrequency.Start();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crystalFrequency.Stop();
            PC.Clear();
            SetSelection(0);
            RegisterMap.ClearRegister();
            RegisterMap.Init();
            registerView.ClearColors();
            Stack.ClearStack();
        }

        private void unterbrechenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crystalFrequency.Stop();
        }

        private void einzelschrittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var index = FindRowForPC(PC.Counter.Value);
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
            setupStack();
        }

        private void setupStack()
        {
            Stack = new Stack();
            StackView = new StackView(Stack);
            StackView.MdiParent = this;
            StackView.Show();
        }

        private void workingRegisterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setupWorkingRegisterBox();   
        }

        private void aRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupRegisterBoxA();
        }

        private void bRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupRegisterBoxB();
        }

        private void optionRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupOptionRegister();
        }

        private void intconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setupIntconRegister();
        }

    }
}
