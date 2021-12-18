using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Dtos
{
    internal sealed class ConnectionRequestDto : IData
    {
        public NetworkProviderDto NetworkProvider { get; internal set; }
        public IEnumerable<Claim> Claims { get; internal set; }

        #region Read/Write Messages
        public static ConnectionRequestDto Read(NetDataReader reader)
        {
            return new ConnectionRequestDto()
            {
                NetworkProvider = NetworkProviderDto.Read(reader),
                Claims = reader.GetClaims()
            };
        }

        public static void Write(NetDataWriter writer, ConnectionRequestDto dto)
        {
            NetworkProviderDto.Write(writer, dto.NetworkProvider);
            writer.Put(dto.Claims);
        }
        #endregion
    }
}
