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
            this.programmLadenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schliessenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datenblattPIC16C84ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.anweisungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einzelschrittToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unterbrechenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deiMuddaToolStripMenuItem,
            this.hilfeToolStripMenuItem,
            this.anweisungenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1056, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deiMuddaToolStripMenuItem
            // 
            this.deiMuddaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.programmLadenToolStripMenuItem,
            this.schliessenToolStripMenuItem});
            this.deiMuddaToolStripMenuItem.Name = "deiMuddaToolStripMenuItem";//TODO
            this.deiMuddaToolStripMenuItem.Size = new System.Drawing.Size(70, 19);
            this.deiMuddaToolStripMenuItem.Text = "Simulator";
            // 
            // programmLadenToolStripMenuItem
            // 
            this.programmLadenToolStripMenuItem.Name = "programmLadenToolStripMenuItem";
            this.programmLadenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.programmLadenToolStripMenuItem.Text = "Programm laden";
            this.programmLadenToolStripMenuItem.Click += new System.EventHandler(this.programmLadenToolStripMenuItem_Click);
            // 
            // schliessenToolStripMenuItem
            // 
            this.schliessenToolStripMenuItem.Name = "schliessenToolStripMenuItem";
            this.schliessenToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
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
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // datenblattPIC16C84ToolStripMenuItem
            // 
            this.datenblattPIC16C84ToolStripMenuItem.Name = "datenblattPIC16C84ToolStripMenuItem";
            this.datenblattPIC16C84ToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.datenblattPIC16C84ToolStripMenuItem.Text = "Datenblatt PIC16F84";
            this.datenblattPIC16C84ToolStripMenuItem.Click += new System.EventHandler(this.datenblattPIC16F84ToolStripMenuItem_Click);
            // 
            // projektToolStripMenuItem
            // 
            this.projektToolStripMenuItem.Name = "projektToolStripMenuItem";
            this.projektToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.projektToolStripMenuItem.Text = "Projekt Aufgabenstellung";
            this.projektToolStripMenuItem.Click += new System.EventHandler(this.projektToolStripMenuItem_Click);
            // 
            // anweisungenToolStripMenuItem
            // 
            this.anweisungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.einzelschrittToolStripMenuItem,
            this.unterbrechenToolStripMenuItem});
            this.anweisungenToolStripMenuItem.Name = "anweisungenToolStripMenuItem";
            this.anweisungenToolStripMenuItem.Size = new System.Drawing.Size(91, 19);
            this.anweisungenToolStripMenuItem.Text = "Anweisungen";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.startToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.startToolStripMenuItem.Text = "Start / Weiter";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.resetToolStripMenuItem.Text = "Zurücksetzen";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // einzelschrittToolStripMenuItem
            // 
            this.einzelschrittToolStripMenuItem.Name = "einzelschrittToolStripMenuItem";
            this.einzelschrittToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.einzelschrittToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.einzelschrittToolStripMenuItem.Text = "Einzelschritt";
            this.einzelschrittToolStripMenuItem.Click += new System.EventHandler(this.einzelschrittToolStripMenuItem_Click);
            // 
            // unterbrechenToolStripMenuItem
            // 
            this.unterbrechenToolStripMenuItem.Name = "unterbrechenToolStripMenuItem";
            this.unterbrechenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.unterbrechenToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.unterbrechenToolStripMenuItem.Text = "Unterbrechen";
            this.unterbrechenToolStripMenuItem.Click += new System.EventHandler(this.unterbrechenToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 392);
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
        private System.Windows.Forms.ToolStripMenuItem programmLadenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem anweisungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einzelschrittToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unterbrechenToolStripMenuItem;

    }
}

