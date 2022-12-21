using DamnOverSharp.Helpers;
using DamnOverSharp.Renderers.Graphic;
using DamnOverSharp.WPF.UiLibrary;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DamnOverSharp.Renderers.WPF
{
    public class WpfRenderer
    {
        public System.Windows.Size RenderSize = new System.Windows.Size(1920, 1080);
        public System.Windows.Point RenderPosition = new System.Windows.Point(0, 0);
        public double DpiX = 96;
        public double DpiY = 96;

        public System.Windows.Point InteractionOffset
        {
            get => InternalWpfInteractionManager.Offset;
            set => InternalWpfInteractionManager.Offset = value;
        }

        internal GraphicRenderer InternalBitmapRenderer;
        internal WpfInteractionManager InternalWpfInteractionManager;
        public Viewbox ViewBox;
        internal Grid MainContentGrid;

        public WpfRenderer(string processName, System.Windows.Size renderSize, System.Windows.Point renderPosition, double dpiX = 96, double dpiY = 96)
        {
            RenderSize = renderSize;
            RenderPosition = renderPosition;
            DpiX = dpiX;
            DpiY = dpiY;

            InternalBitmapRenderer = new GraphicRenderer(processName);
            InternalWpfInteractionManager = new WpfInteractionManager(this);

            ViewBox = new Viewbox() { Width = RenderSize.Width, Height = RenderSize.Height, Stretch = Stretch.None };

            MainContentGrid = new Grid() { Width = RenderSize.Width, Height = RenderSize.Height };
            ViewBox.Child = MainContentGrid;
        }

        public void AddControl(object element)
        {
            if(element is FrameworkElement)
            {
                MainContentGrid.Children.Add(element as FrameworkElement);

                if (element is UserControl)
                {
                    if ((element as UserControl).Content is FrameworkElement)
                    {
                        foreach (VirtualControlBase virtualControl in WPF_VisualHelper.GetAllChildren((element as UserControl).Content as FrameworkElement).OfType<VirtualControlBase>())
                        {
                            virtualControl.SetRendererOnce(this);
                        }
                    }
                }

                if (element is VirtualControlBase)
                {
                    (element as VirtualControlBase).SetRendererOnce(this);

                    foreach (VirtualControlBase virtualControl in WPF_VisualHelper.GetAllChildren(element as VirtualControlBase).OfType<VirtualControlBase>())
                    {
                        virtualControl.SetRendererOnce(this);
                    }
                }
            }
        }

        public void UpdateVisual()
        {
            ViewBox.Measure(RenderSize);
            ViewBox.Arrange(new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            ViewBox.UpdateLayout();

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)RenderSize.Width, (int)RenderSize.Height, DpiX, DpiY, PixelFormats.Pbgra32);
            rtb.Render(ViewBox);

            Bitmap bmp = BitmapSourceToBitmap(rtb);

            Task.Run(() => InternalBitmapRenderer.Draw(bmp, new System.Drawing.Point((int)RenderPosition.X, (int)RenderPosition.Y)));
        }

        private Bitmap BitmapSourceToBitmap(BitmapSource source)
        {
            Bitmap result = new Bitmap(source.PixelWidth, source.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            BitmapData data = result.LockBits(new Rectangle(System.Drawing.Point.Empty, result.Size), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            result.UnlockBits(data);
            return result;
        }

        public void Destroy()
        {
            InternalWpfInteractionManager.Destroy();
            InternalBitmapRenderer.Destroy();

            MainContentGrid = null;
            ViewBox = null;
        }
    }
}
