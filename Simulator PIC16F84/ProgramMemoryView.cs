using System;
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

        public ProgramMemoryView(ProgramMemoryMap UserMemorySpace)
        {
            this.memoryKopie = UserMemorySpace;
            InitializeComponent();
        }

        public void loadProgram(string fileContent)
        {
            programView = splitIntoRows(fileContent);
            programView.RemoveAll(item => item == "");
            extractBinaryCode();
            FillLinesOfDataGrid();

        }

        private List<string> splitIntoRows(string fileContent)
        {
            string[] lines = fileContent.Split( new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            return new List<string>(lines);
        }

        private void extractBinaryCode ()
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void FillLinesOfDataGrid()
        {
            //foreach(var item in memoryKopie.ProgramMemory)
            //{ 
            //    DataGridViewRow row = (DataGridViewRow)this.dataGridView1.Rows[0].Clone();
            //    row.Cells[1].Value = item.Value.ToString("X4");
            //    this.dataGridView1.Rows.Add(row);
            //}

            string temp;

            foreach (var item in programView)
            {
                DataGridViewRow row = (DataGridViewRow)this.dataGridView1.Rows[0].Clone();
                if (item.StartsWith(" ") == false)
                {
                    row.Cells[0].Value = item.Substring(0,4);
                    row.Cells[1].Value = item.Substring(5,4);
                }
                temp = item.Substring(9).Trim();
                row.Cells[2].Value = temp.Substring(0, 5);
                //temp = temp.Substring(6).Trim();
                //TODO: Fortsetzen


                this.dataGridView1.Rows.Add(row);
            }
        }


    }
}
