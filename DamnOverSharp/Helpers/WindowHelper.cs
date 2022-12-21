using System;
using System.Runtime.InteropServices;

namespace DamnOverSharp.Helpers
{
    internal static class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static System.Windows.Rect GetWindowRectangle(IntPtr windowHandle)
        {
            Rect result = new Rect();
            GetWindowRect(windowHandle, ref result);
            return new System.Windows.Rect(result.Left, result.Top, result.Right - result.Left, result.Bottom - result.Top);
        }
    }
}
