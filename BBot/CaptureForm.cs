using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BBot
{
    public partial class CaptureForm : Form
    {
        public Point Coordinate { get; set; }

        public CaptureForm()
        {
            InitializeComponent();
        }

        private void CaptureForm_MouseClick(object sender, MouseEventArgs e)
        {
            this.Coordinate = e.Location;
            DialogResult = DialogResult.OK;
            Dispose();
        }
    }
}
