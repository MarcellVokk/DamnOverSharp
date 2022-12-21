using RenderHookAPI;
using RenderHookAPI.Hook;
using RenderHookAPI.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DamnOverSharp.Renderers.Graphic
{
    public class GraphicRenderer
    {
        internal Process HookedProcess;
        private CaptureProcess CaptureProcess;
        private RenderHookAPI.Hook.Common.ImageElement FrameElement = new RenderHookAPI.Hook.Common.ImageElement();

        public GraphicRenderer(string processName)
        {
            CaptureConfig config = new CaptureConfig()
            {
                Direct3DVersion = Direct3DVersion.AutoDetect,
                ShowOverlay = true
            };

            CaptureInterface captureInterface = new CaptureInterface();
            captureInterface.RemoteMessage += new MessageReceivedEvent((msg) => Debug.WriteLine(msg));

            HookedProcess = Process.GetProcessesByName(processName).First();
            CaptureProcess = new CaptureProcess(HookedProcess, config, captureInterface);
        }

        public void Draw(Bitmap frame, Point topLeft, bool allowTransparency = true)
        {
            Draw(ImageToByteArray(frame, allowTransparency), topLeft);
        }

        public static byte[] ImageToByteArray(Image img, bool allowTransparency = true)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, allowTransparency ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }

        public void Draw(byte[] frame, Point topLeft)
        {
            FrameElement.Location = topLeft;
            FrameElement.Image = frame;

            CaptureProcess.CaptureInterface.DrawOverlayInGame(new RenderHookAPI.Hook.Common.Overlay
            {
                Elements = new List<RenderHookAPI.Hook.Common.IOverlayElement>
                {
                    FrameElement
                }
            });
        }

        public void Destroy()
        {
            HookManager.RemoveHookedProcess(HookedProcess.Id);
            CaptureProcess.Dispose();
            HookedProcess.Dispose();
        }
    }
}
