﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Messages;
using Guppy.Core.Network.Common.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Serialization.NetSerializers
{
    [AutoLoad]
    internal sealed class ConnectionRequestResponseNetSerializer : NetSerializer<ConnectionRequestResponse>
    {
        private INetSerializer<UserDto> _userDtoSerializer = null!;

        public override void Initialize(INetSerializerService serializers)
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
