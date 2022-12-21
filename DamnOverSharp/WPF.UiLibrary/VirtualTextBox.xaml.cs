using CefSharp.DevTools.Network;
using DamnOverSharp.Renderers.WPF;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace DamnOverSharp.WPF.UiLibrary
{
    /// <summary>
    /// Interaction logic for VirtualTextBox.xaml
    /// </summary>
    public partial class VirtualTextBox : VirtualControlBase
    {
        private DispatcherTimer CaretBlink = new DispatcherTimer();
        private bool IsCaretBlinkInvisible = false;

        public VirtualTextBox()
        {
            InitializeComponent();

            CaretBlink.Interval = TimeSpan.FromMilliseconds(600);
            CaretBlink.Tick += CaretBlink_Elapsed;
        }

        private void CaretBlink_Elapsed(object sender, EventArgs args)
        {
            IsCaretBlinkInvisible = !IsCaretBlinkInvisible;

            caret.Visibility = IsCaretBlinkInvisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            UpdateVisual();
        }

        public override bool OnVirtualMouseDown() => true;
        public override bool OnVirtualMouseUp() => true;

        public override void OnGotVirtualFocus()
        {
            CaretIndex = CaretIndex;

            caret.Visibility = System.Windows.Visibility.Visible;
            CaretBlink.Interval = TimeSpan.FromMilliseconds(600);
            CaretBlink.Start();

            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF569DE5"));

            base.OnGotVirtualFocus();
        }

        public override void OnLostVirtualFocus()
        {
            caret.Visibility = System.Windows.Visibility.Hidden;
            CaretBlink.Stop();

            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABADB3"));

            base.OnLostVirtualFocus();
        }

        public override void OnVirtualMouseEnter()
        {
            if (!VirtualFocused)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7EB4EA"));
            }

            base.OnVirtualMouseEnter();
        }

        public override void OnVirtualMouseLeave()
        {
            if (VirtualFocused)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF569DE5"));
            }
            else
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFABADB3"));
            }

            base.OnVirtualMouseLeave();
        }

        public override void OnVirtualKeyDown(Keys keyCode)
        {
            switch (keyCode)
            {
                case Keys.Back:
                    Remove(1);
                    break;
                case Keys.Left:
                    CaretIndex = Math.Max(0, CaretIndex - 1);
                    UpdateVisual();
                    break;
                case Keys.Right:
                    CaretIndex = Math.Min(Text.Length, CaretIndex + 1);
                    UpdateVisual();
                    break;
                case Keys.Space:
                    Insert(" ");
                    break;
                case Keys.Tab:
                    Insert(" ");
                    break;
                case Keys.Oemcomma:
                    Insert(",");
                    break;
                case Keys.OemPeriod:
                    Insert(".");
                    break;
                case Keys.Oemplus:
                    Insert("+");
                    break;
                case Keys.OemMinus:
                    Insert("-");
                    break;
                case Keys.Oemtilde:
                    Insert("`");
                    break;
                case Keys.D0:
                    Insert("0");
                    break;
                case Keys.D1:
                    Insert("1");
                    break;
                case Keys.D2:
                    Insert("2");
                    break;
                case Keys.D3:
                    Insert("3");
                    break;
                case Keys.D4:
                    Insert("4");
                    break;
                case Keys.D5:
                    Insert("5");
                    break;
                case Keys.D6:
                    Insert("6");
                    break;
                case Keys.D7:
                    Insert("7");
                    break;
                case Keys.D8:
                    Insert("8");
                    break;
                case Keys.D9:
                    Insert("9");
                    break;
                case Keys.OemBackslash:
                    Insert(@"\");
                    break;
                case Keys.OemPipe:
                    Insert("|");
                    break;

                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.LWin:
                case Keys.RWin:
                case Keys.Alt:
                case Keys.PrintScreen:
                case Keys.OemCloseBrackets:
                case Keys.OemOpenBrackets:
                    break;

                default:
                    if(keyCode.ToString().Length > 1)
                    {
                        Debug.WriteLine("Key not implemented: " + keyCode.ToString());
                        return;
                    }

                    if (WpfInteractionManager.IsShiftDown)
                    {
                        Insert(char.ToUpper(char.Parse(keyCode.ToString())).ToString());
                    }
                    else
                    {
                        Insert(char.ToLower(char.Parse(keyCode.ToString())).ToString());
                    }
                    break;
            }
        }

        public void Insert(string text)
        {
            _Text = Text.Insert(CaretIndex, text);
            characters.Children.Insert(CaretIndex, new TextBlock() { Text = text, Foreground = Brushes.Black });
            CaretIndex += text.Length;
            UpdateVisual();
        }

        public void Remove(int count)
        {
            if (CaretIndex == 0)
            {
                return;
            }

            count = Math.Min(Text.Length, count);

            _Text = Text.Remove(CaretIndex - count, count);
            characters.Children.RemoveRange(CaretIndex - count, count);
            CaretIndex -= count;
            UpdateVisual();
        }

        private int _CaretIndex = 0;
        public int CaretIndex
        {
            get => _CaretIndex;
            set
            {
                if (VirtualFocused)
                {
                    CaretBlink.Stop();
                    CaretBlink.Interval = TimeSpan.FromMilliseconds(600);
                    CaretBlink.Start();
                    caret.Visibility = System.Windows.Visibility.Visible;
                }

                _CaretIndex = value;
                characters.UpdateLayout();
                content.UpdateLayout();
                caret.Margin = new System.Windows.Thickness(Math.Max(0, Math.Min(characters.ActualWidth - 1, characters.Children.OfType<TextBlock>().ToList().GetRange(0, value).Sum(x => x.ActualWidth))), 0, 0, 0);

                if(characters.ActualWidth <= contentArea.ActualWidth)
                {
                    content.Margin = new System.Windows.Thickness(0, 0, 0, 0);
                    return;
                }
                else if (caret.Margin.Left > Math.Abs(content.Margin.Left) + contentArea.ActualWidth - 5)
                {
                    content.Margin = new System.Windows.Thickness(Math.Min(0, -caret.Margin.Left + contentArea.ActualWidth - 5), 0, 0, 0);
                }
                else if (caret.Margin.Left < Math.Abs(content.Margin.Left) + 5)
                {
                    content.Margin = new System.Windows.Thickness(Math.Min(0, -caret.Margin.Left + 5), 0, 0, 0);
                }

                content.Margin = new System.Windows.Thickness(Math.Max(content.Margin.Left, contentArea.ActualWidth - characters.ActualWidth), 0, 0, 0);
            }
        }

        private string _Text = "";
        public string Text
        {
            get => _Text;
            set
            {
                _Text = value;
                characters.Children.Clear();
                foreach (char c in _Text)
                {
                    characters.Children.Add(new TextBlock() { Text = c.ToString(), Foreground = Brushes.Black });
                }
                CaretIndex = value.Length;
                UpdateVisual();
            }
        }
    }
}
