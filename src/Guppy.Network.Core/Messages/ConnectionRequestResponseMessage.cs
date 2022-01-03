﻿using Guppy.Network.Interfaces;
using Guppy.Network.Security.Dtos;
using Guppy.Threading.Interfaces;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Messages
{
    public class ConnectionRequestResponseMessage : IMessage
    {
        public Boolean Accepted { get; internal set; }
        public UserDto User { get; internal set; }

        #region Read/Write Methods
        public static ConnectionRequestResponseMessage Read(NetDataReader reader, NetworkProvider network)
        {
            Boolean accepted = reader.GetBool();
            UserDto user = null;

            if(reader.GetIf())
            {
                user = UserDto.Read(reader);
            }

            return new ConnectionRequestResponseMessage()
            {
                Accepted = accepted,
                User = user
            };
        }

        public static void Write(NetDataWriter writer, NetworkProvider network, ConnectionRequestResponseMessage dto)
        {
            writer.Put(dto.Accepted);

            if(writer.PutIf(dto.User is not null))
            {
                UserDto.Write(writer, dto.User);
            }
        }
        #endregion
    }
}
