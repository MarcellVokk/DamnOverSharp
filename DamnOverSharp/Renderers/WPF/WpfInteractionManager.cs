using DamnOverSharp.Helpers;
using DamnOverSharp.WPF.UiLibrary;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace DamnOverSharp.Renderers.WPF
{
    internal class WpfInteractionManager
    {
        private WpfRenderer Owner;
        private IKeyboardMouseEvents GlobalMouseHook;

        public Point Offset = new Point(0, 0);
        public static bool IsShiftDown { get; private set; } = false;
        public static bool IsControlDown { get; private set; } = false;
        public static bool IsAltDown { get; private set; } = false;

        public VirtualControlBase FocusedElement { get; private set; } = null;

        internal WpfInteractionManager(WpfRenderer owner)
        {
            Owner = owner;

            GlobalMouseHook = Hook.GlobalEvents();
            
            GlobalMouseHook.MouseDownExt += GlobalMouseDown;
            GlobalMouseHook.MouseUpExt += GlobalMouseUp;
            GlobalMouseHook.MouseMoveExt += GlobalMouseMove;

            GlobalMouseHook.KeyDown += GlobalKeyDown;
            GlobalMouseHook.KeyUp += GlobalKeyUp;
        }

        private void GlobalKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(cursorPos))
            {
                //e.Handled = true;

                switch (e.KeyCode)
                {
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        IsShiftDown = true;
                        break;
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        IsControlDown = true;
                        break;
                    case Keys.Alt:
                        IsAltDown = true;
                        break;
                }

                FocusedElement.OnVirtualKeyDown(e.KeyCode);
            }
        }

        private void GlobalKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(cursorPos))
            {
                //e.Handled = true;

                switch (e.KeyCode)
                {
                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        IsShiftDown = false;
                        break;
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        IsControlDown = false;
                        break;
                    case Keys.Alt:
                        IsAltDown = false;
                        break;
                }

                FocusedElement.OnVirtualKeyUp(e.KeyCode);
            }
        }

        private void GlobalMouseMove(object sender, MouseEventExtArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(MouseHelper.GetCursorPosition()))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                foreach (VirtualControlBase child in WPF_VisualHelper.GetAllChildren(Owner.MainContentGrid).OfType<VirtualControlBase>())
                {
                    if (!child.IsHitTestVisible || child.Visibility != Visibility.Visible || child.Opacity <= 0)
                    {
                        continue;
                    }

                    if (WPF_VisualHelper.GetVisualRect(child, Owner.ViewBox).Contains(relativeCursorPos))
                    {
                        if (!child.VirtualMouseOver)
                        {
                            child.VirtualMouseOver = true;
                            child.OnVirtualMouseEnter();
                        }
                    }
                    else
                    {
                        if (child.VirtualMouseOver)
                        {
                            child.VirtualMouseOver = false;
                            child.OnVirtualMouseLeave();
                        }
                    }

                    child.OnVirtualMouseMove(relativeCursorPos);
                }
            }
        }

        private void GlobalMouseDown(object sender, MouseEventExtArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(cursorPos))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                foreach (FrameworkElement child in WPF_VisualHelper.GetAllChildren(Owner.MainContentGrid).OfType<FrameworkElement>())
                {
                    if (!child.IsHitTestVisible || child.Visibility != Visibility.Visible || child.Opacity <= 0)
                    {
                        continue;
                    }

                    if (WPF_VisualHelper.GetVisualRect(child, Owner.ViewBox).Contains(relativeCursorPos))
                    {
                        e.Handled = true;

                        if (child is VirtualControlBase)
                        {
                            if((child as VirtualControlBase).OnVirtualMouseDown())
                            {
                                if(FocusedElement != (child as VirtualControlBase))
                                {
                                    if (FocusedElement != null)
                                    {
                                        FocusedElement.VirtualFocused = false;
                                        FocusedElement.OnLostVirtualFocus();
                                    }

                                    FocusedElement = (child as VirtualControlBase);
                                    FocusedElement.VirtualFocused = true;
                                    FocusedElement.OnGotVirtualFocus();

                                    Owner.UpdateVisual();
                                }

                                (child as VirtualControlBase).VirtualMouseDown = true;

                                break;
                            }
                        }
                    }
                }
            }
        }

        private void GlobalMouseUp(object sender, MouseEventExtArgs e)
        {
            Rect windowRect = WindowHelper.GetWindowRectangle(Owner.InternalBitmapRenderer.HookedProcess.MainWindowHandle);
            Point cursorPos = MouseHelper.GetCursorPosition();

            if (windowRect.Contains(cursorPos))
            {
                Point relativeCursorPos = new Point(cursorPos.X - windowRect.X - Offset.X, cursorPos.Y - windowRect.Y - Offset.Y);

                foreach (FrameworkElement child in WPF_VisualHelper.GetAllChildren(Owner.MainContentGrid).OfType<FrameworkElement>())
                {
                    if (!child.IsHitTestVisible || child.Visibility != Visibility.Visible || child.Opacity <= 0)
                    {
                        continue;
                    }

                    if (WPF_VisualHelper.GetVisualRect(child, Owner.ViewBox).Contains(relativeCursorPos))
                    {
                        e.Handled = true;

                        if (child is VirtualControlBase)
                        {
                            if ((child as VirtualControlBase).OnVirtualMouseUp())
                            {
                                (child as VirtualControlBase).VirtualMouseDown = false;

                                break;
                            }
                        }
                    }
                }
            }
        }

        public void Destroy()
        {
            GlobalMouseHook.MouseDownExt -= GlobalMouseDown;
            GlobalMouseHook.MouseUpExt -= GlobalMouseUp;
            GlobalMouseHook.MouseMoveExt -= GlobalMouseMove;

            GlobalMouseHook.Dispose();
        }
    }
}
