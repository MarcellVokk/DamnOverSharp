using RenderHookAPI.Hook.Common;
using System;

namespace RenderHookAPI.Interface
{
    /// <summary>
    /// Note: Deliberately not using MarshalByRefObj
    /// </summary>
    [Serializable]
    public class DrawOverlayEventArgs
    {
        public IOverlay Overlay { get; set; }

        public bool IsUpdatePending { get; set; }

        public DrawOverlayEventArgs()
        {
        }
    }
}
