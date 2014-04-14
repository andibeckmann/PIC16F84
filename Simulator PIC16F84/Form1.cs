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
        public Main()
        {
            InitializeComponent();
            IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;

            RegisterFileMap RegisterMap = new RegisterFileMap();
            RegisterView registerView = new RegisterView(RegisterMap);
            // Set the Parent Form of the Child window.
            registerView.MdiParent = this;
            // Display the new form.
            registerView.Show();

            ProgramMemoryMap UserMemorySpace = new ProgramMemoryMap();
            ProgramMemoryView programMemoryView = new ProgramMemoryView(UserMemorySpace);
            programMemoryView.MdiParent = this;
            programMemoryView.Show();

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

    }
}
