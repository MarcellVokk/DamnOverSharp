using System;
using System.Windows.Media;

namespace DamnOverSharp.WPF.UiLibrary
{
    /// <summary>
    /// Interaction logic for VirtualButton.xaml
    /// </summary>
    public partial class VirtualButton : VirtualControlBase
    {
        public VirtualButton()
        {
            InitializeComponent();
        }

        public event EventHandler Clicked;

        public override void OnVirtualMouseEnter()
        {
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFBEE6FD"));
            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3C7FB1"));

            base.OnVirtualMouseEnter();
        }

        public override void OnVirtualMouseLeave()
        {
            border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
            border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF707070"));

            base.OnVirtualMouseLeave();
        }

        public override bool OnVirtualMouseDown()
        {
            Clicked?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public string Text
        {
            get => text.Text;
            set
            {
                text.Text = value;
                UpdateVisual();
            }
        }
    }
}
