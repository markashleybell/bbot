using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace BBot
{
    public partial class MainForm : Form
    {
        private Bitmap d;
        private static string[,] grid = new string[8, 8];
        private const int cellSize = 40;
        private Point topLeft;
        private Point origin;
        private Size size;

        private bool debugMode;

        private System.Windows.Forms.Timer tMove = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer tDuration = new System.Windows.Forms.Timer();

       

        public MainForm()
        {
            InitializeComponent();

            debugMode = true;

            size = new Size(320, 320);

            tMove.Tick += new EventHandler(tMove_Tick);
            tMove.Interval = 125;
            tMove.Enabled = false;
            tMove.Stop();

            tDuration.Tick += new EventHandler(tDuration_Tick);
            tDuration.Interval = 61000;
            tDuration.Enabled = true;
            tDuration.Stop();

            // Application. = Cursors.Cross;

            WIN32.RegisterHotKey(Handle, 100, WIN32.KeyModifiers.Control | WIN32.KeyModifiers.Windows, Keys.S);

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 0);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WIN32.UnregisterHotKey(Handle, 100);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            switch (m.Msg)
            {
                case WM_HOTKEY:
                    tMove.Stop();
                    tMove.Enabled = false;
                    tDuration.Stop();
                    tDuration.Enabled = false;
                    break;
            }
            base.WndProc(ref m);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (Graphics graphics = Graphics.FromImage(d))
            {
                graphics.CopyFromScreen(origin.X, origin.Y, 0, 0, size);
            }
            
            ScanGrid();
            DoMoves();
             
            tMove.Enabled = true;
            tMove.Start();

            tDuration.Start();
        }

        void tDuration_Tick(object sender, EventArgs e)
        {
            tMove.Stop();
            tMove.Enabled = false;

            tDuration.Stop();
            tDuration.Enabled = false;
        }

        void tMove_Tick(object sender, EventArgs e)
        {
            using (Graphics graphics = Graphics.FromImage(d))
            {
                graphics.CopyFromScreen(origin.X, origin.Y, 0, 0, size);
            }

            ScanGrid();
            DoMoves();
        }

        public bool MatchColours(Color a, Color b)
        {
            if (a.R > 230 && b.R > 230)
                return true;

            if (a.G > 230 && b.G > 230)
                return true;

            if (a.B > 230 && b.B > 230)
                return true;


            return false;
        }

        private void DoMoves()
        {
            var s = topLeft;

            // Across
            for (int y = 0; y < 8; y++)
            {
                // Down
                for (int x = 0; x < 8; x++)
                {
                    // x
                    // -
                    // x
                    // x

                    if (y + 3 < 8)
                    {
                        if (grid[x, y] == grid[x, y + 2] && grid[x, y + 2] == grid[x, y + 3])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y)) + cellSize);
                            Mouse.Release();
                        }
                    }

                    // - x
                    // x
                    // x

                    if (x > 0 && y + 2 < 8)
                    {
                        if (grid[x, y] == grid[x - 1, y + 1] && grid[x - 1, y + 1] == grid[x - 1, y + 2])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) - cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }
                   


                    // x
                    // - x
                    // x

                    if (x - 1 > 0 && y - 1 >= 0 && y + 1 < 8)
                    {
                        if (grid[x, y] == grid[x - 1, y - 1] && grid[x - 1, y - 1] == grid[x - 1, y + 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) - cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }


                    // x
                    // x
                    // - x

                    if (x - 1 > 0 && y - 2 > 0)
                    {
                        if (grid[x, y] == grid[x - 1, y - 1] && grid[x - 1, y - 1] == grid[x - 1, y - 2])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) - cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }



                    // x
                    // x
                    // -
                    // x


                    if (y - 3 > 0)
                    {
                        if (grid[x, y] == grid[x, y - 2] && grid[x, y - 2] == grid[x, y - 3])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y)) - cellSize);
                            Mouse.Release();
                        }
                    }


                    // x -
                    //   x
                    //   x

                    if (x + 1 < 8 && y + 2 < 8)
                    {
                        if (grid[x, y] == grid[x + 1, y + 1] && grid[x + 1, y + 1] == grid[x + 1, y + 2])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) + cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }



                    //   x
                    // x -
                    //   x


                    if (x + 1 < 8 && y - 1 > 0 && y + 1 < 8)
                    {
                        if (grid[x, y] == grid[x + 1, y - 1] && grid[x + 1, y - 1] == grid[x + 1, y + 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) + cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }


                    //   x
                    //   x
                    // x -

                    if (x + 1 < 8 && y - 2 > 0)
                    {
                        if (grid[x, y] == grid[x + 1, y - 1] && grid[x + 1, y - 1] == grid[x + 1, y - 2])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) + cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }


                    // xx-x

                    if (x - 3 > 0 && y < 8)
                    {
                        if (grid[x, y] == grid[x - 2, y] && grid[x - 2, y] == grid[x - 3, y])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) - cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }


                    // x--
                    // -xx

                    if (x + 2 < 8 && y + 1 < 8)
                    {
                        if (grid[x, y] == grid[x + 1, y + 1] && grid[x + 1, y + 1] == grid[x + 2, y + 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y) + cellSize));
                            Mouse.Release();
                        }
                    }

                   


                    // -x-
                    // x-x

                    if (x + 1 < 8 && x - 1 > 0 && y + 1 < 8)
                    {
                        if (grid[x, y] == grid[x + 1, y + 1] && grid[x + 1, y + 1] == grid[x - 1, y + 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y) + cellSize));
                            Mouse.Release();
                        }
                    }


                    // --x
                    // xx-

                    if (x - 2 > 0 && y + 1 < 8)
                    {
                        if (grid[x, y] == grid[x - 1, y + 1] && grid[x - 1, y + 1] == grid[x - 2, y + 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y) + cellSize));
                            Mouse.Release();
                        }
                    }


                    // x-xx

                    if (x + 3 < 8 && y < 8)
                    {
                        if (grid[x, y] == grid[x + 2, y] && grid[x + 2, y] == grid[x + 3, y])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo((s.X + (cellSize * x) + cellSize), s.Y + (cellSize * y));
                            Mouse.Release();
                        }
                    }


                    // -xx
                    // x--

                    if (x + 2 < 8 && y - 1 > 0)
                    {
                        if (grid[x, y] == grid[x + 1, y - 1] && grid[x + 1, y - 1] == grid[x + 2, y - 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y) - cellSize));
                            Mouse.Release();
                        }
                    }



                    // x-x
                    // -x-

                    if (x -1 > 0 && x + 1 < 8 && y - 1 > 0)
                    {
                        if (grid[x, y] == grid[x - 1, y - 1] && grid[x - 1, y - 1] == grid[x + 1, y - 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y) - cellSize));
                            Mouse.Release();
                        }
                    }


                    // xx-
                    // --x

                    if (x - 2 > 0 && y - 2 > 0)
                    {
                        if (grid[x, y] == grid[x - 1, y - 1] && grid[x - 1, y - 1] == grid[x - 2, y - 1])
                        {
                            Mouse.MoveTo(s.X + (cellSize * x), s.Y + (cellSize * y));
                            Mouse.Press();
                            Mouse.MoveTo(s.X + (cellSize * x), (s.Y + (cellSize * y) - cellSize));
                            Mouse.Release();
                        }
                    }
                    
                }
            }
        }

        private void ScanGrid()
        {
            richTextBox1.Clear();

            // Add roughly half a cell X and Y so we get the centre of the gem (ish)
            // Just changing the top coordinate got me my first million point game...
            // It seems to match colours better in that part of the gem
            int top = 12; // + (cellSize / 2);
            int left = 18; // + (cellSize / 2);

            // Across
            for (int y = 0; y < 8; y++)
            {
                // Down
                for (int x = 0; x < 8; x++)
                {
                    int t = (top + (cellSize * y));
                    int l = (left + (cellSize * x));

                    Color c = d.GetPixel(l, t);
                    d.SetPixel(l, t, Color.Red);
                    pictureBox1.Image = d;
                    grid[x, y] = c.ToArgb().ToString();

                    richTextBox1.AppendText("Row " + y + ", Col " + x + " [" + l + ", " + t + "]: " + grid[x, y] + System.Environment.NewLine);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cf = new CaptureForm();
            if (cf.ShowDialog() == DialogResult.OK)
            {
                var point = cf.Coordinate; // Property in form2

                point.Y += 19;

                label3.Text = "x: " + point.X + ", y:" + point.Y;
                origin = point;
                topLeft = new Point(point.X + 18, point.Y + 18);
                // Size size is how big an area to capture
                // pointOrigin is the upper left corner of the area to capture

                d = new Bitmap(size.Width, size.Height);

                using (Graphics graphics = Graphics.FromImage(d))
                {
                    graphics.CopyFromScreen(point.X, point.Y, 0, 0, size);
                }
                label5.Text = d.GetPixel(5, 5).ToString();
                ScanGrid();
                pictureBox1.Image = d;
            }
        }
    }
}
