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
    internal sealed class UserDto : IPacket
    {
        public readonly Int32 UserId;

        public UserDto(Int32 userId)
        {
            this.UserId = userId;
        }

        internal static UserDto Read(NetDataReader reader, NetworkProvider network)
        {
            return new UserDto(reader.GetInt());
        }

        internal static void Write(NetDataWriter writer, NetworkProvider network, UserDto dto)
        {
            writer.Put(dto.UserId);
        }
    }
}
