﻿using Guppy.Example.Library.Layerables;
using Guppy.Network;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Messages
{
    public sealed class PositionDto : IPacket
    {
        public readonly Vector2 Position;
        public readonly Vector2 Velocity;

        public PositionDto(Positionable positionable)
        {
            this.Position = positionable.Position;
            this.Velocity = positionable.Velocity;
        }
        private PositionDto(Vector2 position, Vector2 velocity)
        {
            this.Position = position;
            this.Velocity = velocity;
        }

        #region Read/Write Methods
        internal static PositionDto Read(NetDataReader reader, NetworkProvider network)
        {
            return new PositionDto(
                reader.GetVector2(), 
                reader.GetVector2());
        }

        internal static void Write(NetDataWriter writer, NetworkProvider network, PositionDto dto)
        {
            writer.Put(dto.Position);
            writer.Put(dto.Velocity);
        }
        #endregion
    }
}