using Guppy.Network.Interfaces;
using Guppy.Network.Security;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.Structs;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Dtos
{
    public class UserDto
    {
        public Int32 Id { get; internal set; }
        public IEnumerable<Claim> Claims { get; internal set; }

        #region Read/Write Methods
        public static UserDto Read(NetDataReader reader)
        {
            return new UserDto()
            {
                Id = reader.GetInt(),
                Claims = reader.GetClaims()
            };
        }

        public static void Write(NetDataWriter writer, UserDto dto)
        {
            writer.Put(dto.Id);
            writer.Put(dto.Claims);
        }
        #endregion
    }
}
