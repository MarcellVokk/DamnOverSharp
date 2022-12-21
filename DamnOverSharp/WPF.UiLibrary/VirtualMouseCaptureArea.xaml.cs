using System;
using System.Diagnostics;
using System.Windows;

namespace DamnOverSharp.WPF.UiLibrary
{
    /// <summary>
    /// Interaction logic for VirtualMouseCaptureArea.xaml
    /// </summary>
    public partial class VirtualMouseCaptureArea : VirtualControlBase
    {
        public VirtualMouseCaptureArea()
        {
            InitializeComponent();
        }

        public event EventHandler<Point> VirtualMouseDrag;

        public override void OnVirtualMouseMove(Point position)
        {
            if (VirtualMouseDown)
            {
                VirtualMouseDrag?.Invoke(this, position);
            }
        }

        public override bool OnVirtualMouseUp() => true;
        public override bool OnVirtualMouseDown() => true;
    }
}
