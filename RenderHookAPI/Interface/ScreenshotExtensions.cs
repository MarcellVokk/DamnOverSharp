﻿using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace RenderHookAPI.Interface
{
    public static class ScreenshotExtensions
    {
        public static Bitmap ToBitmap(this byte[] data, int width, int height, int stride, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                var img = new Bitmap(width, height, stride, pixelFormat, handle.AddrOfPinnedObject());
                return img;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        public static Bitmap ToBitmap(this Screenshot screenshot)
        {
            if (screenshot.Format == ImageFormat.PixelData)
            {
                return screenshot.Data.ToBitmap(screenshot.Width, screenshot.Height, screenshot.Stride, screenshot.PixelFormat);
            }
            else
            {
                return screenshot.Data.ToBitmap();
            }
        }

        public static Bitmap ToBitmap(this byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream(imageBytes);
            try
            {
                return (Bitmap)Image.FromStream(ms);
            }
            catch
            {
                return null;
            }
        }

        public static byte[] ToByteArray(this Image img, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, format);
                stream.Close();
                return stream.ToArray();
            }
        }
    }
}
