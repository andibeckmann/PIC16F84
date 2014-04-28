﻿using System;
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

namespace Simulator_PIC16F84
{
    public partial class Main : Form
    {
        RegisterFileMap RegisterMap;
        ProgramMemoryMap UserMemorySpace;
        ProgramMemoryView ProgramView;
        WorkingRegister WorkingRegister;

        public Main()
        {
            InitializeComponent();
            IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            RegisterMap = new RegisterFileMap();
            RegisterView registerView = new RegisterView(ref RegisterMap);
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

            WorkingRegister = new WorkingRegister();

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

            System.IO.StreamReader sr = new System.IO.StreamReader(testPath + "SimTest1.lst");
            string fileContent = sr.ReadToEnd();
            sr.Close();
            ProgramView.loadProgram(fileContent);
            UserMemorySpace = ProgramView.getBinaryCode();

            for (int index = 0; index < UserMemorySpace.getLength(); index++ )
            {
                UserMemorySpace.ProgramMemory[index].DecodeInstruction(WorkingRegister);
            }
        }

    }
}
