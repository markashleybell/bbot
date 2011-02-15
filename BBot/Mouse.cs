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
using System.Text;
using System.Runtime.InteropServices;

namespace BBot
{
    // As we don't have the Java.awt.Robot class like those lucky Java people,
    // here's a utility class to simplify performing mouse actions
    public class Mouse
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);
        
        // Flags to represent mouse actions
        private const uint LEFTDOWN = 0x00000002;
        private const uint LEFTUP = 0x00000004;
        private const uint MIDDLEDOWN = 0x00000020;
        private const uint MIDDLEUP = 0x00000040;
        private const uint MOVE = 0x00000001;
        private const uint ABSOLUTE = 0x00008000;
        private const uint RIGHTDOWN = 0x00000008;
        private const uint RIGHTUP = 0x00000010;

        public static void MoveTo(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void Press()
        {
            mouse_event(LEFTDOWN, 0, 0, 0, 0);
        }

        public static void Release()
        {
            mouse_event(LEFTUP, 0, 0, 0, 0);
        }

        public static void Click()
        {
            mouse_event(LEFTDOWN, 0, 0, 0, 0);
            mouse_event(LEFTUP, 0, 0, 0, 0);
        }
    }
}
