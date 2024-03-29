﻿using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Identity.Dtos;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using LiteNetLib.Utils;

namespace Guppy.Network.Serialization.NetSerializers
{
    [AutoLoad]
    internal sealed class ConnectionRequestResponseNetSerializer : NetSerializer<ConnectionRequestResponse>
    {
        private INetSerializer<UserDto> _userDtoSerializer = null!;

        public override void Initialize(INetSerializerProvider serializers)
        {
            base.Initialize(serializers);

            _userDtoSerializer = serializers.Get<UserDto>();
        }

        public override ConnectionRequestResponse Deserialize(NetDataReader reader)
        {
            ConnectionRequestResponseType type = reader.GetEnum<ConnectionRequestResponseType>();
            UserDto? systemUser = type == ConnectionRequestResponseType.Accepted ? _userDtoSerializer.Deserialize(reader) : null;
            UserDto? currentUser = type == ConnectionRequestResponseType.Accepted ? _userDtoSerializer.Deserialize(reader) : null;

            return new ConnectionRequestResponse()
            {
                Type = type,
                SystemUser = systemUser,
                CurrentUser = currentUser
            };
        }

        public override void Serialize(NetDataWriter writer, in ConnectionRequestResponse instance)
        {
            writer.Put(instance.Type);

            if (instance.Type == ConnectionRequestResponseType.Accepted)
            {
                _userDtoSerializer.Serialize(writer, instance.SystemUser!);
                _userDtoSerializer.Serialize(writer, instance.CurrentUser!);
            }
        }
    }
}
