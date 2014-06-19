using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulator_PIC16F84.Views
{
    public partial class CrystalFrequenceView : Form
    {
        private TrackBar slider;
        private Label label;
        public Main PIC;
      
        public CrystalFrequenceView(Main PIC, int currentCrystalFrequence)
        {
            InitializeComponent();
            this.PIC = PIC;
            InitializeSlider(currentCrystalFrequence);
            InitializeLabel(currentCrystalFrequence);
            this.Controls.Add(slider);
            this.Controls.Add(label);
        }

        private void InitializeLabel(int currentCrystalFrequence)
        {
            label = new Label();
            label.Location = new Point(0, 45);
            label.Size = new System.Drawing.Size(60, 20);
            label.Text = currentCrystalFrequence.ToString() + " ns";
        }

        private void InitializeSlider(int currentCrystalFrequence)
        {
            slider = new TrackBar();
            slider.Location = new Point(0, 0);
            slider.Size = new System.Drawing.Size(224, 45);
            slider.Maximum = 1000;
            slider.Minimum = 200;
            slider.TickFrequency = 10;
            slider.LargeChange = 5;
            slider.SmallChange = 2;
            slider.Value = currentCrystalFrequence;
            slider.Scroll += new System.EventHandler(crystalFrequenceChanged);
        }

        private void crystalFrequenceChanged(object sender, EventArgs e)
        {
            label.Text = slider.Value.ToString() + " ns";
            PIC.crystalFrequency = slider.Value;
        }
    }
}
