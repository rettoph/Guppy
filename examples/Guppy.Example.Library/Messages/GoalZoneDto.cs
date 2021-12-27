using Guppy.Network;
using Guppy.Network.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Example.Library.Messages
{
    public sealed class GoalZoneDto : IPacket
    {
        public readonly UInt16 OwnerNetworkId;

        public readonly Single X;
        public readonly Single Y;
        public readonly Single Width;
        public readonly Single Height;

        public GoalZoneDto(ushort ownerNetworkId, float x, float y, float width, float height)
        {
            OwnerNetworkId = ownerNetworkId;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        internal static GoalZoneDto Read(NetDataReader reader, NetworkProvider network)
        {
            return new GoalZoneDto(
                ownerNetworkId: reader.GetUShort(), 
                x: reader.GetFloat(), 
                y: reader.GetFloat(), 
                width: reader.GetFloat(), 
                height: reader.GetFloat());
        }

        internal static void Write(NetDataWriter writer, NetworkProvider network, GoalZoneDto dto)
        {
            writer.Put(dto.OwnerNetworkId);

            writer.Put(dto.X);
            writer.Put(dto.Y);
            writer.Put(dto.Width);
            writer.Put(dto.Height);
        }
    }
}
