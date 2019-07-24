using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.Lidgren
{
    public static class NetOutgoingMessageExtensions
    {
        #region Guid Methods
        public static void Write(this NetOutgoingMessage om, Guid guid)
        {
            om.Write(guid.ToByteArray());
        }
        #endregion

        #region Color Methods
        public static void Write(this NetOutgoingMessage om, Color color)
        {
            om.Write(color.PackedValue);
        }
        #endregion

        #region Vector2 Methods
        public static void Write(this NetOutgoingMessage om, Vector2 vector2)
        {
            om.Write(vector2.X);
            om.Write(vector2.Y);
        }
        #endregion

        #region Entity Methods
        public static void Write(this NetOutgoingMessage om, NetworkEntity entity)
        {
            if (om.WriteExists(entity))
            {
                om.Write(entity.Id);
            }
        }
        #endregion

        #region IfNull Methods
        public static Boolean WriteIf(this NetOutgoingMessage om, Boolean value)
        {
            if (value)
            {
                om.Write(true);
                return true;
            }
            else
            {
                om.Write(false);
                return false;
            }
        }
        public static Boolean WriteExists(this NetOutgoingMessage om, Object value)
        {
            if (value == null)
            {
                om.Write(false);
                return false;
            }
            else
            {
                om.Write(true);
                return true;
            }
        }
        #endregion
    }
}
