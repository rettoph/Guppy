using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Messages
{
    internal sealed class ConnectionRequestMessage : IData
    {
        public NetworkProviderMessage NetworkProvider { get; internal set; }
        public IEnumerable<Claim> Claims { get; internal set; }

        #region Read/Write Messages
        public static ConnectionRequestMessage Read(NetDataReader reader)
        {
            return new ConnectionRequestMessage()
            {
                NetworkProvider = NetworkProviderMessage.Read(reader),
                Claims = reader.GetClaims()
            };
        }

        public static void Write(NetDataWriter writer, ConnectionRequestMessage dto)
        {
            NetworkProviderMessage.Write(writer, dto.NetworkProvider);
            writer.Put(dto.Claims);
        }
        #endregion
    }
}
