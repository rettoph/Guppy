﻿using Guppy.Attributes;
using Guppy.Network.Enums;
using Guppy.Network.Identity.Dtos;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using LiteNetLib.Utils;

namespace Guppy.Network.Serialization.NetSerializers
{
    [AutoLoad]
    internal sealed class UserActionNetSerializer : NetSerializer<UserAction>
    {
        private INetSerializer<UserDto> _userDtoSerializer;

        public override void Initialize(INetSerializerProvider serializers)
        {
            base.Initialize(serializers);

            _userDtoSerializer = serializers.Get<UserDto>();
        }

        public override UserAction Deserialize(NetDataReader reader)
        {
            UserAction instance = new UserAction()
            {
                Type = reader.GetEnum<UserActionTypes>(),
                UserDto = _userDtoSerializer.Deserialize(reader)
            };

            return instance;
        }

        public override void Serialize(NetDataWriter writer, in UserAction instance)
        {
            writer.Put(instance.Type);
            _userDtoSerializer.Serialize(writer, instance.UserDto);
        }
    }
}
