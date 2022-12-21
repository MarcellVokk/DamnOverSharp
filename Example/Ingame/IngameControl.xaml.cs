using DamnOverSharp.WPF.UiLibrary;
using System;
using System.Windows;

namespace Example
{
    /// <summary>
    /// Interaction logic for IngameControl.xaml
    /// </summary>
    public partial class IngameControl : VirtualControlBase
    {
        public IngameControl()
        {
            InitializeComponent();
        }

        private void VirtualButton_Clicked(object sender, EventArgs e)
        {
            button.Text = "Clicked :)";
        }

        private void mouseCapture_VirtualMouseDrag(object sender, System.Windows.Point e)
        {
            if (mouseCapture.VirtualMouseDown)
            {
                this.Margin = new Thickness(e.X - mouseCapture.ActualWidth / 2, e.Y - mouseCapture.ActualHeight / 2, 0, 0);
                UpdateVisual();
            }
        }
    }
}
