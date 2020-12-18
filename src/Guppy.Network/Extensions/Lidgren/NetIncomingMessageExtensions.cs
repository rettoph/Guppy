using Guppy.Lists;
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
        public static Guid PeekGuid(this NetIncomingMessage im)
        {
            return new Guid(im.PeekBytes(16));
        }
        public static Guid ReadGuid(this NetIncomingMessage im)
        {
            return new Guid(im.ReadBytes(16));
        }
        #endregion

        #region Enum Methods
        public static T ReadEnum<T>(this NetIncomingMessage im)
            where T : Enum
        {
            var byteVal = im.ReadByte();
            return EnumHelper.GetValues<T>().First(v => Convert.ToByte(v) == byteVal);
        }
        #endregion

        #region Color Methods
        public static Color PeekColor(this NetIncomingMessage im)
        {
            return new Color(im.PeekUInt32());
        }
        public static Color ReadColor(this NetIncomingMessage im)
        {
            return new Color(im.ReadUInt32());
        }
        #endregion

        #region Vector2 Methods
        public static Vector2 PeekVector2(this NetIncomingMessage im)
        {
            var v = new Vector2(im.ReadSingle(), im.ReadSingle());
            im.Position -= 64;

            return v;
        }
        public static void ReadVector2(this NetIncomingMessage im, ref Vector2 vector2)
        {
            vector2.X = im.ReadSingle();
            vector2.Y = im.ReadSingle();
        }
        public static Vector2 ReadVector2(this NetIncomingMessage im)
        {
            return new Vector2(im.ReadSingle(), im.ReadSingle());
        }
        #endregion

        #region IfNull Methods
        public static Boolean ReadExists(this NetIncomingMessage im)
        {
            return im.ReadBoolean();
        }
        #endregion

        #region Entity Methods
        public static T ReadEntity<T>(this NetIncomingMessage im, EntityList entities)
            where T : Entity
        {
            if (im.ReadExists())
                return entities.GetById<T>(im.ReadGuid());

            return default(T);
        }

        public static T ReadEntity<T>(this NetIncomingMessage im, EntityList entities, Action<NetIncomingMessage, T> ifExists)
            where T : Entity
        {
            if (im.ReadExists())
            {
                var entity = entities.GetById<T>(im.ReadGuid());
                ifExists(im, entity);

                return entity;
            }

            return default(T);
        }
        #endregion
    }
}
