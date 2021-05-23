using Guppy.Utilities;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Extensions.Lidgren
{
    public static class NetIncomingMessageExtensions
    {
        #region Guid Methods
        public static Vector2 PeekVector2(this NetIncomingMessage im)
        {
            var bytes = im.PeekBytes(16);
            return new Vector2(BitConverter.ToSingle(bytes, 0), BitConverter.ToSingle(bytes, 4));
        }
        public static Vector2 ReadVector2(this NetIncomingMessage im)
        {
            return new Vector2(im.ReadSingle(), im.ReadSingle());
        }
        #endregion
    }
}
