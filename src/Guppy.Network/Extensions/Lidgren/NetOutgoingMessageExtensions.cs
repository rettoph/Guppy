using Guppy.Extensions.System;
using Guppy.Network.Utilities.Messages;
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

        #region Enum Methods
        public static void Write<T>(this NetOutgoingMessage om, T value)
            where T : Enum
                => om.Write(Convert.ToByte(value));
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

        #region MessageType Methods
        /// <summary>
        /// Write a signed message with custom data.
        /// 
        /// This will encode the recieved <paramref name="type"/> then invoke the
        /// recieved <paramref name="writer"/>.
        /// 
        /// This is used for passing custom data, and can be processed
        /// by a <see cref="MessageManager"/> instance.
        /// </summary>
        /// <param name="om"></param>
        /// <param name="type"></param>
        /// <param name="writer"></param>
        public static NetOutgoingMessage Write(this NetOutgoingMessage om, UInt32 type, Action<NetOutgoingMessage> writer)
        {
            om.Write(type);
            writer(om);
            return om;
        }
        #endregion

        #region Entity Methods
        public static void Write(this NetOutgoingMessage om, Entity entity)
        {
            if (om.WriteExists(entity))
                om.Write(entity.Id);
        }
        public static void Write<TEntity>(this NetOutgoingMessage om, TEntity entity, Action<NetOutgoingMessage, TEntity> ifExists)
            where TEntity : Entity
        {
            if (om.WriteExists(entity))
            {
                om.Write(entity.Id);
                ifExists(om, entity);
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
