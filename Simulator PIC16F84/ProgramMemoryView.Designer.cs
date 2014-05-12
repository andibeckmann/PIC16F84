namespace Simulator_PIC16F84
{
    partial class ProgramMemoryView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code



        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.BreakPoints = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BreakPoints,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(784, 462);
            this.dataGridView1.TabIndex = 0;
            // 
            // BreakPoints
            // 
            this.BreakPoints.HeaderText = "Stop";
            this.BreakPoints.Name = "BreakPoints";
            this.BreakPoints.ReadOnly = true;
            this.BreakPoints.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.BreakPoints.Width = 35;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Zeile";
            this.Column1.Name = "Column1";
            this.Column1.ToolTipText = "Binärcode-Programmzeile";
            this.Column1.Width = 40;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Code";
            this.Column2.Name = "Column2";
            this.Column2.ToolTipText = "Binärcode-Instruktionen";
            this.Column2.Width = 40;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "QZeile";
            this.Column3.Name = "Column3";
            this.Column3.ToolTipText = "Quellcode-Zeile";
            this.Column3.Width = 40;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Punkt";
            this.Column4.Name = "Column4";
            this.Column4.ToolTipText = "Quellcode-Assemblersprungpunkt";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Assemblercode";
            this.Column5.Name = "Column5";
            this.Column5.ToolTipText = "Quellcode-Assemblerinstruktionen";
            this.Column5.Width = 150;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Kommentar";
            this.Column6.Name = "Column6";
            this.Column6.ToolTipText = "Quellcode-Assemblerkommentare";
            this.Column6.Width = 335;
            // 
            // ProgramMemoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 462);
            this.Controls.Add(this.dataGridView1);
            this.Location = new System.Drawing.Point(265, 0);
            this.Name = "ProgramMemoryView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ProgramMemoryView";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn BreakPoints;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}