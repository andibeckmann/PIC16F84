﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Simulator_PIC16F84
{
    public partial class ProgramMemoryView : Form
    {
        private List<string> programView = new List<string>();
        ProgramMemoryMap memoryKopie;

        public EventHandler<int> HandleBreakpoint;

        public ProgramMemoryView(ProgramMemoryMap UserMemorySpace, Main MainView)
        {
            this.memoryKopie = UserMemorySpace;
            InitializeComponent();
            this.AutoScroll = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            this.dataGridView1.SelectionChanged += SelectionChangedEvent;
            this.dataGridView1.CellDoubleClick += HandleBreakPoints;
            HandleBreakpoint += MainView.HandleBreakpoint;
        }

        private void SelectionChangedEvent(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                var index = dataGridView1.CurrentCell.RowIndex;
                if (index != -1)
                {
                    dataGridView1.Rows[index].Selected = true;
                }
            }
        }

        private void HandleBreakPoints(object sender, EventArgs e)
        {
            var index = dataGridView1.CurrentCell.RowIndex;
            if (dataGridView1.CurrentCell.ColumnIndex.Equals(0) && dataGridView1.CurrentCell.RowIndex != -1)
            {
                if (dataGridView1[1, index].Value != null)
                {
                    var counter = int.Parse(dataGridView1[1, index].Value.ToString(), System.Globalization.NumberStyles.HexNumber);
                    if (this.HandleBreakpoint != null)
                    {
                        this.HandleBreakpoint(this, index);
                    }
                    if (dataGridView1[0, index].Style.BackColor == Color.Red)
                    {
                        dataGridView1[0, index].Style.BackColor = Color.White;
                    }
                    else
                    {
                        dataGridView1[0, index].Style.BackColor = Color.Red;
                    }
                }
            }
        }

        public void loadProgram(string fileContent)
        {
            programView = splitIntoRows(fileContent);
            programView.RemoveAll(item => item == "");
            extractBinaryCode();
            ClearDataGrid();
            FillLinesOfDataGrid();

        }

        private void ClearDataGrid()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Refresh();
        }

        private List<string> splitIntoRows(string fileContent)
        {
            string[] lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            return new List<string>(lines);
        }

        private void extractBinaryCode()
        {
            int memoryAdress;
            int binaryCode;

            foreach (var item in programView)
            {
                if (item.StartsWith(" "))
                    continue;
                else
                {
                    memoryAdress = Convert.ToInt32(item.Substring(0, 4), 16);
                    binaryCode = Convert.ToInt32(item.Substring(5, 4), 16);
                    memoryKopie.ProgramMemory[memoryAdress].Value = binaryCode;
                }
            }
        }

        public ProgramMemoryMap getBinaryCode()
        {
            return memoryKopie;
        }

        private void FillLinesOfDataGrid()
        {
            foreach (var item in programView)
            {
                DataGridViewRow row = (DataGridViewRow)this.dataGridView1.Rows[0].Clone();
                ExtractProcessorCode(item, row);
                ExtractSourceCode(item, row);
                this.dataGridView1.Rows.Add(row);
            }
        }

        private static void ExtractSourceCode(string item, DataGridViewRow row)
        {
            string temp;
            temp = ExtractSourceCodeLineNumbers(item, row);
            if (temp.Length > 5)
            {
                temp = ExtractJumpPointDesignation(row, temp);
                temp = ExtractAssemblerCode(row, temp);
                ExtractComments(row, temp);
            }
        }

        private static string ExtractSourceCodeLineNumbers(string item, DataGridViewRow row)
        {
            string temp;
            temp = item.Substring(9).Trim();
            row.Cells[3].Value = temp.Substring(0, 5);
            return temp;
        }

        private static string ExtractJumpPointDesignation(DataGridViewRow row, string temp)
        {
            temp = temp.Substring(7);
            if (temp.StartsWith(" ") == false)
            {
                row.Cells[4].Value = temp;
                return null;
            }
            return temp.Substring(8);
        }

        private static string ExtractAssemblerCode(DataGridViewRow row, string temp)
        {
            if (temp == null)
                return null;
            temp = temp.Trim();
            if (temp.Contains(";") == true)
            {
                int index = temp.IndexOf(";");
                if (index > 0)
                {
                    row.Cells[5].Value = temp.Substring(0, index);
                    temp = temp.Substring(index);
                }
                return temp;
            }
            else
                row.Cells[5].Value = temp;
            return null;
        }

        private static void ExtractComments(DataGridViewRow row, string temp)
        {
            if (temp != null)
                row.Cells[6].Value = temp;
        }

        private static void ExtractProcessorCode(string item, DataGridViewRow row)
        {
            if (item.StartsWith(" ") == false)
            {
                row.Cells[1].Value = item.Substring(0, 4);
                row.Cells[2].Value = item.Substring(5, 4);
            }
        }


    }
}
