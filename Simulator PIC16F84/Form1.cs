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

namespace Simulator_PIC16F84
{
    public partial class Main : Form
    {
        RegisterFileMap RegisterMap;
        ProgramMemoryMap UserMemorySpace;
        ProgramMemoryView ProgramView;
        WorkingRegister W;
        ProgramCounter PC;
        Stack Stack;
        WatchdogTimer WDT;
        Prescaler Prescaler;
        RegisterView registerView;
        System.Timers.Timer crystalFrequency; 

        public Main()
        {
            InitializeComponent();
            IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            RegisterMap = new RegisterFileMap();
            registerView = new RegisterView(ref RegisterMap);
            // Set the Parent Form of the Child window.
            registerView.MdiParent = this;
            registerView.Size = new Size { Height = this.Size.Height - 150, Width = 275 };
            // Display the new form.
            registerView.Show();
            var size = registerView.Size;

            UserMemorySpace = new ProgramMemoryMap();
            ProgramView = new ProgramMemoryView(UserMemorySpace);
            ProgramView.MdiParent = this;
            ProgramView.SetDesktopLocation(size.Width + 18, 0);
            ProgramView.Show();

            W = new WorkingRegister(RegisterMap);
            PC = new ProgramCounter();
            Stack = new Stack();
            WDT = new WatchdogTimer();
            Prescaler = new Prescaler();
            crystalFrequency = new System.Timers.Timer(10);
            crystalFrequency.Elapsed += new System.Timers.ElapsedEventHandler(ExecuteCycle);


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
            //programmLaden.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Programme";
            //programmLaden.Filter = "lst files (*.lst)|*.lst";
            //programmLaden.FilterIndex = 1;
            //programmLaden.RestoreDirectory = false;

            //if (programmLaden.ShowDialog() == DialogResult.OK)
            //{
            //    try
            //    {
            //        System.IO.StreamReader sr = new System.IO.StreamReader(programmLaden.FileName);
            //        string fileContent = sr.ReadToEnd();
            //        sr.Close();

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error: Could not read file" + ex.Message);
            //    }
            //}

            String testPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Programme\\";

            System.IO.StreamReader sr = new System.IO.StreamReader(testPath + "SimTest2.lst");
            string fileContent = sr.ReadToEnd();
            sr.Close();
            ProgramView.loadProgram(fileContent);
            UserMemorySpace = ProgramView.getBinaryCode();
            PC.Clear();
        }

        private void ExecuteCycle(object source, ElapsedEventArgs e)
        {
            var index = FindRowForPC(PC.Counter.Value);
            this.registerView.ClearColors();
            SetSelection(index);
            UserMemorySpace.ProgramMemory[PC.Counter.Value].DecodeInstruction( RegisterMap, W, PC, Stack, WDT, Prescaler);
            PC.InkrementPC();
        }

        private void SetSelection(int index)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { SetSelection(index); });
            }
            else
            {
                this.ProgramView.dataGridView1.ClearSelection();
                this.ProgramView.dataGridView1.Rows[index].Selected = true;
                this.ProgramView.dataGridView1.CurrentCell = this.ProgramView.dataGridView1[0, index];
            }
        }

        private int FindRowForPC(int counter)
        {
            int rowIndex = 0;
            foreach (DataGridViewRow row in this.ProgramView.dataGridView1.Rows)
            {
                if(row.Cells[0].Value == null)
                {
                    continue;
                }
                if (row.Cells[0].Value.ToString().Equals(counter.ToString("X4")))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            return rowIndex;
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crystalFrequency.Start();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crystalFrequency.Stop();
            PC.Clear();
            registerView.ClearRegister();
            SetSelection(0);
        }

        private void unterbrechenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            crystalFrequency.Stop();
        }

        private void einzelschrittToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteCycle(null, null);
        }

    }
}
