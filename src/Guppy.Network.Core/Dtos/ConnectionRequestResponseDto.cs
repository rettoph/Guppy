using Guppy.Network.Interfaces;
using Guppy.Network.Security.Dtos;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Dtos
{
    public class ConnectionRequestResponseDto : IData
    {
        public Boolean Accepted { get; internal set; }
        public UserDto User { get; internal set; }

        #region Read/Write Methods
        public static ConnectionRequestResponseDto Read(NetDataReader reader)
        {
            Boolean accepted = reader.GetBool();
            UserDto user = null;

            if(reader.GetIf())
            {
                user = UserDto.Read(reader);
            }

            return new ConnectionRequestResponseDto()
            {
                Accepted = accepted,
                User = user
            };
        }

        public static void Write(NetDataWriter writer, ConnectionRequestResponseDto dto)
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
