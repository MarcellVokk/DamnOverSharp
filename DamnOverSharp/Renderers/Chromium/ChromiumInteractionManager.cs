using CefSharp;
using DamnOverSharp.Helpers;
using Gma.System.MouseKeyHook;
using System.Windows;

namespace DamnOverSharp.Renderers.Chromium
{
    internal class ChromiumInteractionManager
    {
        private ChromiumRenderer Owner;
        private IKeyboardMouseEvents GlobalInputHook;

        public Point Offset = new Point(0, 0);

        internal ChromiumInteractionManager(ChromiumRenderer owner)
        {
            Owner = owner;

            GlobalInputHook = Hook.GlobalEvents();
            
            GlobalInputHook.MouseDownExt += GlobalMouseDown;
            GlobalInputHook.MouseUpExt += GlobalMouseUp;
            GlobalInputHook.MouseWheel += GlobalMouseWheel;

            GlobalInputHook.KeyDown += GlobalKeyDown;
            GlobalInputHook.KeyUp += GlobalKeyUp;
            GlobalInputHook.KeyPress += GlobalKeyPress;
        }

        private void GlobalKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                Owner.InternalBrowser.GetBrowser().GetHost().SendKeyEvent(new KeyEvent() { WindowsKeyCode = (int)e.KeyChar, Type = KeyEventType.Char });
            }
        }

        private void GlobalKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                Owner.InternalBrowser.GetBrowser().GetHost().SendKeyEvent(new KeyEvent() { WindowsKeyCode = (int)e.KeyCode, Type = KeyEventType.KeyDown });
            }
        }

        private void GlobalKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                Owner.InternalBrowser.GetBrowser().GetHost().SendKeyEvent(new KeyEvent() { WindowsKeyCode = (int)e.KeyCode, Type = KeyEventType.KeyUp });
            }
        }

        public void GlobalMouseMove(object sender, MouseEventExtArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                if (Owner.InternalBrowser.IsBrowserInitialized)
                {
                    Owner.InternalBrowser.GetBrowser().GetHost().SendMouseMoveEvent((int)relativeCursorPos.X, (int)relativeCursorPos.Y, false, CefEventFlags.IsLeft);
                }
            }
        }

        private void GlobalMouseDown(object sender, MouseEventExtArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                if(e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Owner.InternalBrowser.GetBrowser().GetHost().SendMouseClickEvent((int)relativeCursorPos.X, (int)relativeCursorPos.Y, MouseButtonType.Left, false, 1, CefEventFlags.None);
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    Owner.InternalBrowser.GetBrowser().GetHost().SendMouseClickEvent((int)relativeCursorPos.X, (int)relativeCursorPos.Y, MouseButtonType.Right, false, 1, CefEventFlags.None);
                }
            }
        }

        private void GlobalMouseUp(object sender, MouseEventExtArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Owner.InternalBrowser.GetBrowser().GetHost().SendMouseClickEvent((int)relativeCursorPos.X, (int)relativeCursorPos.Y, MouseButtonType.Left, true, 1, CefEventFlags.None);
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    Owner.InternalBrowser.GetBrowser().GetHost().SendMouseClickEvent((int)relativeCursorPos.X, (int)relativeCursorPos.Y, MouseButtonType.Right, true, 1, CefEventFlags.None);
                }
            }
        }

        private void GlobalMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                if (!Owner.InternalBrowser.IsBrowserInitialized)
                {
                    return;
                }

                Owner.InternalBrowser.GetBrowser().GetHost().SendMouseWheelEvent((int)relativeCursorPos.X, (int)relativeCursorPos.Y, 0, e.Delta, CefEventFlags.None);
            }
        }

        public void Destroy()
        {
            GlobalInputHook.MouseDownExt -= GlobalMouseDown;
            GlobalInputHook.MouseUpExt -= GlobalMouseUp;
            //GlobalMouseHook.MouseMoveExt -= GlobalMouseMove;
            GlobalInputHook.MouseWheelExt -= GlobalMouseWheel;

            GlobalInputHook.Dispose();
        }
    }
}
