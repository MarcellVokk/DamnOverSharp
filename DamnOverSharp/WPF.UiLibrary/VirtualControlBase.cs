using DamnOverSharp.Renderers.WPF;
using System.Windows;
using System.Windows.Controls;

namespace DamnOverSharp.WPF.UiLibrary
{
    public class VirtualControlBase : UserControl
    {
        private WpfRenderer Renderer = null;

        public bool VirtualMouseOver { get; internal set; } = false;
        public bool VirtualMouseDown { get; internal set; } = false;
        public bool VirtualFocused { get; internal set; } = false;

        internal void SetRendererOnce(WpfRenderer renderer)
        {
            if(Renderer == null)
            {
                Renderer = renderer;
            }
        }

        public void UpdateVisual()
        {
            Renderer?.UpdateVisual();
        }

        public virtual void OnGotVirtualFocus() => UpdateVisual();
        public virtual void OnLostVirtualFocus() => UpdateVisual();

        public virtual void OnVirtualMouseEnter() => UpdateVisual();

        public virtual void OnVirtualMouseLeave() => UpdateVisual();

        public virtual bool OnVirtualMouseDown() => false;

        public virtual bool OnVirtualMouseUp() => false;

        public virtual void OnVirtualMouseMove(Point position) { }

        public virtual void OnVirtualKeyDown(System.Windows.Forms.Keys keyCode) { }
        public virtual void OnVirtualKeyUp(System.Windows.Forms.Keys keyCode) { }
    }
}
