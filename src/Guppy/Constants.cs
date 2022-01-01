using Guppy.Threading.Utilities;
using Guppy.Utilities.Cameras;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public static class Constants
    {
        public static class ServiceNames
        {
            public static readonly String TransientSpritebatch = "service:spritebatch:transient";
            public static readonly String TransientCamera = "service:camera:transient";
            public static readonly String GameMessageBus = "message-queue:game";
        }

        public static class Content
        {
            public const String DebugFont = "guppy:font:debug";
        }

        public static class BusQueues
        {
            public static readonly Bus.Queue ReleaseServiceQueue = new Bus.Queue("release-service-queue", Int32.MaxValue);
        }
    }
}
