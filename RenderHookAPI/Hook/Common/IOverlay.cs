using System.Collections.Generic;

namespace RenderHookAPI.Hook.Common
{
    public interface IOverlay: IOverlayElement
    {
        List<IOverlayElement> Elements { get; set; }
    }
}
