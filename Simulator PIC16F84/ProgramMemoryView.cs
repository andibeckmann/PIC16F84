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

        }

        private List<string> splitIntoRows(string fileContent)
        {
            string[] lines = fileContent.Split( new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            return new List<string>(lines);
        }

        private void extractBinaryCode ()
        {
            int memoryAdress;
            byte binaryCode;

            foreach (var item in programView)
            {
                if (item.StartsWith(" "))
                    continue;
                else
                {
                    if (int.TryParse(item.Substring(0, 4), out memoryAdress))
                    {
                        if (byte.TryParse(item.Substring(5, 4), out binaryCode))
                        {
                            memoryKopie.ProgramMemory[memoryAdress].Value = binaryCode;
                        }
                    }
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
            foreach(var item in memoryKopie.ProgramMemory)
            { 
                DataGridViewRow row = (DataGridViewRow)this.dataGridView1.Rows[0].Clone();
                row.Cells["Column0"].Value = item;
                this.dataGridView1.Rows.Add(row);
            }
        }


    }
}
