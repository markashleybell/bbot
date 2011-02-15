using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BBot
{
    public class Mouse
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInfo);

        // use this flags

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
