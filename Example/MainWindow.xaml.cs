using DamnOverSharp.Renderers.Chromium;
using DamnOverSharp.Renderers.Graphic;
using DamnOverSharp.Renderers.WPF;
using System.Windows;

namespace Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WpfRenderer Renderer;
        ChromiumRenderer ChromiumRenderer;
        GraphicRenderer BitmapRenderer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Renderer?.Destroy();
            Renderer = null;
            ChromiumRenderer?.Destroy();
            ChromiumRenderer = null;
            BitmapRenderer?.Destroy();
            BitmapRenderer = null;
        }

        private void Button_ShowWPFOverlay_Clicked(object sender, RoutedEventArgs e)
        {
            Renderer?.Destroy();
            Renderer = null;
            ChromiumRenderer?.Destroy();
            ChromiumRenderer = null;
            BitmapRenderer?.Destroy();
            BitmapRenderer = null;

            Renderer = new WpfRenderer(textbox_targetApp.Text, new Size(1920, 1080), new Point(0, 0)) { InteractionOffset = new Point(8, 30) };
            Renderer.AddControl(new IngameControl() { VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10, 10, 0, 0) });
            Renderer.UpdateVisual();
        }

        private void Button_ShowChromiumOverlay_Clicked(object sender, RoutedEventArgs e)
        {
            Renderer?.Destroy();
            Renderer = null;
            ChromiumRenderer?.Destroy();
            ChromiumRenderer = null;
            BitmapRenderer?.Destroy();
            BitmapRenderer = null;

            ChromiumRenderer = new ChromiumRenderer(textbox_targetApp.Text, new Size(1920, 1080), new Point(0, 0), false) { InteractionOffset = new Point(0, 0) };
        }

        private void Button_ShowBitmapOverlay_Clicked(object sender, RoutedEventArgs e)
        {
            Renderer?.Destroy();
            Renderer = null;
            ChromiumRenderer?.Destroy();
            ChromiumRenderer = null;
            BitmapRenderer?.Destroy();
            BitmapRenderer = null;

            BitmapRenderer = new GraphicRenderer(textbox_targetApp.Text);

            using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1920, 1080))
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.FillRectangle(System.Drawing.Brushes.Orange, 50, 50, 300, 300);

                BitmapRenderer.Draw(bmp, new System.Drawing.Point(0, 0));
            }
        }
    }
}
