using Guppy.Extensions.System.Collections;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Extensions.Lidgren
{
    public static class NetOutgoingMessageExtensions
    {
        #region Guid Methods
        public static void Write(this NetOutgoingMessage om, Vector2 vector)
        {
            om.Write(vector.X);
            om.Write(vector.Y);
        }
        #endregion
    }
}
