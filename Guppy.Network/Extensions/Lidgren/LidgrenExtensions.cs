﻿using Guppy.Collections;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Extensions.Lidgren
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

        #region Vector2 Methods
        public static void Write(this NetOutgoingMessage om, Vector2 vector2)
        {
            om.Write(vector2.X);
            om.Write(vector2.Y);
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

        #region Entity Methods
        public static void Write(this NetOutgoingMessage om, NetworkEntity entity)
        {
            if (entity == null)
            {
                om.Write(false);
            }
            else
            {
                om.Write(true);
                om.Write(entity.Id);
            }
        }
        public static void ReadEntity<TEntity>(this NetIncomingMessage im, ref TEntity entity, EntityCollection entities)
            where TEntity : NetworkEntity
        {
            entity = im.ReadEntity<TEntity>(entities);
        }
        public static TEntity ReadEntity<TEntity>(this NetIncomingMessage im, EntityCollection entities)
            where TEntity : NetworkEntity
        {
            if (im.ReadBoolean())
                return entities.GetById<TEntity>(
                    im.ReadGuid());

            return null;
        }
        #endregion
    }
}
