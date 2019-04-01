using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions
{
    public static class LidgrenExtensions
    {
        #region Guid Methods
        public static void Write(this NetOutgoingMessage om, Guid guid)
        {
            om.Write(guid.ToByteArray());
        }
        public static Guid ReadGuid(this NetIncomingMessage im)
        {
            return new Guid(im.ReadBytes(16));
        }
        #endregion

        #region Color Methods
        public static void Write(this NetOutgoingMessage om, Color color)
        {
            om.Write(color.PackedValue);
        }
        public static Color ReadColor(this NetIncomingMessage im)
        {
            return new Color(im.ReadUInt32());
        }
        #endregion
    }
}
