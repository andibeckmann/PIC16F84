﻿namespace Simulator_PIC16F84
{
    partial class Main
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.deiMuddaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schliessenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datenblattPIC16C84ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deiMuddaToolStripMenuItem,
            this.hilfeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(621, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deiMuddaToolStripMenuItem
            // 
            this.deiMuddaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.schliessenToolStripMenuItem});
            this.deiMuddaToolStripMenuItem.Name = "deiMuddaToolStripMenuItem";
            this.deiMuddaToolStripMenuItem.Size = new System.Drawing.Size(70, 19);
            this.deiMuddaToolStripMenuItem.Text = "Simulator";
            // 
            // schliessenToolStripMenuItem
            // 
            this.schliessenToolStripMenuItem.Name = "schliessenToolStripMenuItem";
            this.schliessenToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.schliessenToolStripMenuItem.Text = "Schließen";
            this.schliessenToolStripMenuItem.Click += new System.EventHandler(this.schliessenToolStripMenuItem_Click);
            // 
            // hilfeToolStripMenuItem
            // 
            this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.datenblattPIC16C84ToolStripMenuItem,
            this.projektToolStripMenuItem});
            this.hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
            this.hilfeToolStripMenuItem.Size = new System.Drawing.Size(44, 19);
            this.hilfeToolStripMenuItem.Text = "Hilfe";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // datenblattPIC16C84ToolStripMenuItem
            // 
            this.datenblattPIC16C84ToolStripMenuItem.Name = "datenblattPIC16C84ToolStripMenuItem";
            this.datenblattPIC16C84ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.datenblattPIC16C84ToolStripMenuItem.Text = "Datenblatt PIC16C84";
            this.datenblattPIC16C84ToolStripMenuItem.Click += new System.EventHandler(this.datenblattPIC16C84ToolStripMenuItem_Click);
            // 
            // projektToolStripMenuItem
            // 
            this.projektToolStripMenuItem.Name = "projektToolStripMenuItem";
            this.projektToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.projektToolStripMenuItem.Text = "Projekt Aufgabenstellung";
            this.projektToolStripMenuItem.Click += new System.EventHandler(this.projektToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 230);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simulator PIC16F84";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deiMuddaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schliessenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem datenblattPIC16C84ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projektToolStripMenuItem;

    }
}

