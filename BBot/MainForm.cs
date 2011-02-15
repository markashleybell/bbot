/*
The MIT License

Copyright (c) 2011 Mark Ashley Bell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Configuration;
using System.Reflection;

namespace BBot
{
    public partial class MainForm : Form
    {
        private Bitmap capturedArea; // Holds the captured area bitmap for each iteration

        private static Size size = new Size(320, 320); // The size of the Bejeweled gem grid
        private const int cellSize = 40; // Size of each cell in the grid

        // When we sample a pixel from within a cell we'll offset it by these amounts
        // Add roughly half a cell X and Y so we get the centre of the gem (ish)
        // Just changing the top coordinate got me my first million point game...
        // It seems to match colours better in that part of the gem
        private const int topOffset = 12;
        private const int leftOffset = 18;

        private static Color[,] grid = new Color[8, 8]; // Matrix to hold the colour present in each grid cell

        private Point origin; // 
        private Point startPoint;

        private bool debugMode = false;

        private System.Windows.Forms.Timer tMove = new System.Windows.Forms.Timer(); // Timer that performs the moves
        private System.Windows.Forms.Timer tDuration = new System.Windows.Forms.Timer(); // Timer that stops the loop after a certain duration

        public MainForm()
        {
            InitializeComponent();

            debugMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);

            // Set up the timer that performs the moves
            tMove.Tick += new EventHandler(tMove_Tick);
            tMove.Interval = 125; // Perform a move every N milliseconds
            tMove.Enabled = true;
            tMove.Stop();

            // This is the timer that stops the loop after a certain duration
            tDuration.Tick += new EventHandler(tDuration_Tick);
            tDuration.Enabled = true;
            tDuration.Stop();

            // Shift-Ctrl-Alt Escape will exit the play loop
            WIN32.RegisterHotKey(Handle, 100, WIN32.KeyModifiers.Control | WIN32.KeyModifiers.Alt | WIN32.KeyModifiers.Shift, Keys.Escape);

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            // Put the window at top right
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Width, 0);

            // Initially set the preview image
            this.preview.Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("BBot.Assets.Instruction.bmp"));

            if (debugMode)
                this.Height = 734;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Kill the system-wide hotkey on app exit
            WIN32.UnregisterHotKey(Handle, 100);
        }

        // Set up hotkeys: we need one to be able to quit the loop because 
        // while the bot is running the mouse is hijacked
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;

            switch (m.Msg)
            {
                case WM_HOTKEY:
                    tMove.Stop();
                    tDuration.Stop();
                    break;
            }

            base.WndProc(ref m);
        }

        // Stop the play loop automatically at the end of the specified duration
        private void tDuration_Tick(object sender, EventArgs e)
        {
            tMove.Stop();
            tDuration.Stop();
        }

        // This gets fired every N seconds in order to perform moves
        private void tMove_Tick(object sender, EventArgs e)
        {
            CaptureArea();
            ScanGrid(false);
            DoMoves();
        }

        // Capture the specified screen area which we set up earlier
        private void CaptureArea()
        {
            using (Graphics graphics = Graphics.FromImage(capturedArea))
            {
                graphics.CopyFromScreen(origin.X, origin.Y, 0, 0, size);
            }
        }

        // Check if two colours match
        private bool MatchColours(Color a, Color b)
        {
            return (a.ToArgb().ToString() == b.ToArgb().ToString());

            // TODO: Sort this out so that we match special gem colours by range
            /*
            // White
            if (a.R > 230 && a.G > 230 && a.B > 230)
                return (b.R > 230 && b.G > 230 && b.B > 230);

            // Yellow
            if (a.R > 230 && a.G > 180 && a.B < 100)
                return (b.R > 230 && b.G > 180 && b.B < 100);

            // Orange
            if (a.R > 230 && a.G > 230 && a.B > 100)
                return (b.R > 230 && b.G > 230 && b.B > 100);

            // Purple
            if (a.R > 230 && a.B > 230)
                return (b.R > 230 && b.B > 230);

            // Red
            if (a.R > 230)
                return (b.R > 230);

            // Green
            if (a.G > 230)
                return (b.G > 230);

            // Blue
            if (a.B > 230)
                return (b.B > 230);

            return false;
            */
        }

        // 
        // Code adapted from William Henry's Java bot: http://mytopcoder.com/bejeweledBot
        // TODO: I did this the easy way by always moving the cell we are currently looking at (just to get the app running), but this isn't the most efficient method
        private void DoMoves()
        {
            var s = startPoint;

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
                        if (MatchColours(grid[x, y], grid[x, y + 2]) && MatchColours(grid[x, y + 2], grid[x, y + 3]))
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
                        if (MatchColours(grid[x, y], grid[x - 1, y + 1]) && MatchColours(grid[x - 1, y + 1], grid[x - 1, y + 2]))
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
                        if (MatchColours(grid[x, y], grid[x - 1, y - 1]) && MatchColours(grid[x - 1, y - 1], grid[x - 1, y + 1]))
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
                        if (MatchColours(grid[x, y], grid[x - 1, y - 1]) && MatchColours(grid[x - 1, y - 1], grid[x - 1, y - 2]))
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
                        if (MatchColours(grid[x, y], grid[x, y - 2]) && MatchColours(grid[x, y - 2], grid[x, y - 3]))
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
                        if (MatchColours(grid[x, y], grid[x + 1, y + 1]) && MatchColours(grid[x + 1, y + 1], grid[x + 1, y + 2]))
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
                        if (MatchColours(grid[x, y], grid[x + 1, y - 1]) && MatchColours(grid[x + 1, y - 1], grid[x + 1, y + 1]))
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
                        if (MatchColours(grid[x, y], grid[x + 1, y - 1]) && MatchColours(grid[x + 1, y - 1], grid[x + 1, y - 2]))
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
                        if (MatchColours(grid[x, y], grid[x - 2, y]) && MatchColours(grid[x - 2, y], grid[x - 3, y]))
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
                        if (MatchColours(grid[x, y], grid[x + 1, y + 1]) && MatchColours(grid[x + 1, y + 1], grid[x + 2, y + 1]))
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
                        if (MatchColours(grid[x, y], grid[x + 1, y + 1]) && MatchColours(grid[x + 1, y + 1], grid[x - 1, y + 1]))
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
                        if (MatchColours(grid[x, y], grid[x - 1, y + 1]) && MatchColours(grid[x - 1, y + 1], grid[x - 2, y + 1]))
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
                        if (MatchColours(grid[x, y], grid[x + 2, y]) && MatchColours(grid[x + 2, y], grid[x + 3, y]))
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
                        if (MatchColours(grid[x, y], grid[x + 1, y - 1]) && MatchColours(grid[x + 1, y - 1], grid[x + 2, y - 1]))
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
                        if (MatchColours(grid[x, y], grid[x - 1, y - 1]) && MatchColours(grid[x - 1, y - 1], grid[x + 1, y - 1]))
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
                        if (MatchColours(grid[x, y], grid[x - 1, y - 1]) && MatchColours(grid[x - 1, y - 1], grid[x - 2, y - 1]))
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

        // Scan the gem grid and capture a coloured pixel from each cell
        private void ScanGrid(bool showCentres)
        {
            if(debugMode)
                debugConsole.Clear();

            int top = topOffset;
            int left = leftOffset;

            // Across
            for (int y = 0; y < 8; y++)
            {
                // Down
                for (int x = 0; x < 8; x++)
                {
                    int t = (top + (cellSize * y));
                    int l = (left + (cellSize * x));

                    // Capture a colour from this pixel
                    Color c = capturedArea.GetPixel(l, t);
                    
                    // Store it in the grid matrix at the correct position
                    grid[x, y] = c;

                    // Mark the position we are sampling on the preview for debugging
                    if(showCentres)
                        capturedArea.SetPixel(l, t, Color.Red);

                    if (debugMode)
                    {
                        // Refresh the preview each iteration and write out all matrix contents (slow, hence we only do it in debug mode)
                        preview.Image = capturedArea;
                        debugConsole.AppendText("Row " + y + ", Col " + x + " [" + l + ", " + t + "]: " + grid[x, y] + System.Environment.NewLine);
                    }
                }
            }
        }

        // Show the capture form and allow user to locate gem grid
        private void captureButton_Click(object sender, EventArgs e)
        {
            var cf = new CaptureForm();

            if (cf.ShowDialog() == DialogResult.OK)
            {
                // size is how big an area of the screen to capture
                // origin is the upper left corner of the area to capture
                // startPoint is the point we start sampling colour pixels from within that area
                // capturedArea holds the bitmap data for the current iteration

                origin = cf.Coordinate; // Get the coordinate clicked in the capture form and set it as the origin

                startPoint = new Point(origin.X + leftOffset, origin.Y + topOffset);

                capturedArea = new Bitmap(size.Width, size.Height);

                CaptureArea();
                ScanGrid(true);

                // Show the preview so user can check selection is correct
                preview.Image = capturedArea;
            }
        }
        
        // Start the play loop
        private void playButton_Click(object sender, EventArgs e)
        {
            CaptureArea();
            ScanGrid(false);
            DoMoves();

            tMove.Start();
            tDuration.Interval = (int)duration.Value * 1000;
            tDuration.Start();
        }
    }
}
