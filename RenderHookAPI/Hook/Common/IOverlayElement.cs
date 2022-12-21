using System;

namespace RenderHookAPI.Hook.Common
{
    public interface IOverlayElement : ICloneable
    {
        bool Hidden { get; set; }

        void Frame();
    }
}
