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
            public static readonly String GameMessageQueue = "message-queue:game";
            public static readonly String SceneMessagueQueue = "message-queue:scene";
        }

        public static class Content
        {
            public const String DebugFont = "guppy:font:debug";
        }
    }
}
