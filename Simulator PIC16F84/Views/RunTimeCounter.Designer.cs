namespace Simulator_PIC16F84
{
    partial class RunTimeCounter
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
            this.Value = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Value
            // 
            this.Value.AutoSize = true;
            this.Value.Location = new System.Drawing.Point(5, 10);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(150, 13);
            this.Value.TabIndex = 0;
            // 
            // RunTimeCounter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 31);
            this.Controls.Add(this.Value);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RunTimeCounter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "RunTimeCounter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Value;
    }
}