using System;
using RenderHookAPI.Interface;

namespace RenderHookAPI.Hook
{
    internal interface IDXHook: IDisposable
    {
        CaptureInterface Interface
        {
            get;
            set;
        }
        CaptureConfig Config
        {
            get;
            set;
        }

        ScreenshotRequest Request
        {
            get;
            set;
        }

        void Hook();

        void Cleanup();
    }
}
