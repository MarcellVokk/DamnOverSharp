using CefSharp;
using CefSharp.OffScreen;
using DamnOverSharp.Renderers.Graphic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace DamnOverSharp.Renderers.Chromium
{
    public class ChromiumRenderer
    {
        public System.Windows.Size RenderSize = new System.Windows.Size(1920, 1080);
        public System.Windows.Point RenderPosition = new System.Windows.Point(0, 0);
        public bool AllowTransparency { get; private set; } = true;
        public System.Windows.Point InteractionOffset
        {
            get => InternalChromiumInteractionManager.Offset;
            set => InternalChromiumInteractionManager.Offset = value;
        }

        internal GraphicRenderer InternalBitmapRenderer;
        internal ChromiumInteractionManager InternalChromiumInteractionManager;
        internal ChromiumWebBrowser InternalBrowser;

        private bool IsDisposed = false;

        public ChromiumRenderer(string processName, System.Windows.Size renderSize, System.Windows.Point renderPosition, bool allowTransparency)
        {
            RenderSize = renderSize;
            RenderPosition = renderPosition;
            AllowTransparency = allowTransparency;

            InternalBitmapRenderer = new GraphicRenderer(processName);
            InternalChromiumInteractionManager = new ChromiumInteractionManager(this);

            CefSettings settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(settings);

            InternalBrowser = new ChromiumWebBrowser(address: "https://www.google.com") { Size = new Size((int)renderSize.Width, (int)renderSize.Height) };

            Task.Run(() =>
            {
                while (!IsDisposed)
                {
                    UpdateVisual();
                    Thread.Sleep(1);
                }
            });

            Task.Run(() =>
            {
                while (!IsDisposed)
                {
                    InternalChromiumInteractionManager.GlobalMouseMove(null, null);
                    Thread.Sleep(1);
                }
            });
        }

        public void UpdateVisual()
        {
            if (InternalBrowser != null && InternalBrowser.IsBrowserInitialized)
            {
                using (Bitmap bmp = InternalBrowser.ScreenshotOrNull())
                {
                    if (bmp != null)
                    {
                        InternalBitmapRenderer.Draw(bmp, new Point((int)RenderPosition.X, (int)RenderPosition.Y), AllowTransparency);
                    }
                }
            }
        }

        public void Destroy()
        {
            IsDisposed = true;
            Thread.Sleep(100);

            InternalChromiumInteractionManager.Destroy();
            InternalBitmapRenderer.Destroy();
        }
    }
}
