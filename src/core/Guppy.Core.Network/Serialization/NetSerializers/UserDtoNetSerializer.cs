﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Network.Identity.Claims;
using Guppy.Core.Network.Identity.Dtos;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Serialization.NetSerializers
{
    [AutoLoad]
    internal sealed class UserDtoNetSerializer : NetSerializer<UserDto>
    {
        public override UserDto Deserialize(NetDataReader reader)
        {
            UserDto instance = new UserDto()
            {
                Id = reader.GetInt(),
                Claims = new Claim[reader.GetInt()]
            };

            for (var i = 0; i < instance.Claims.Length; i++)
            {
                instance.Claims[i] = Claim.Deserialize(reader);
            }

            return instance;
        }

        public override void Serialize(NetDataWriter writer, in UserDto instance)
        {
            writer.Put(instance.Id);
            writer.Put(instance.Claims.Length);

            foreach (Claim claim in instance.Claims)
            {
                claim.Serialize(writer);
            }
        }
    }
}
