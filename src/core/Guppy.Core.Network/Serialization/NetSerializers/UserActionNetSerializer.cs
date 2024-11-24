using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Messages;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Serialization.NetSerializers
{
    internal sealed class UserActionNetSerializer : NetSerializer<UserAction>
    {
        private INetSerializer<UserDto> _userDtoSerializer = null!;

        public override void Initialize(INetSerializerService serializers)
        {
            base.Initialize(serializers);

            _userDtoSerializer = serializers.Get<UserDto>();
        }

        public override UserAction Deserialize(NetDataReader reader)
        {
            UserAction instance = new()
            {
                GroupId = reader.GetByte(),
                Type = reader.GetEnum<UserActionTypes>(),
                UserDto = _userDtoSerializer.Deserialize(reader)
            };

            return instance;
        }

        public override void Serialize(NetDataWriter writer, in UserAction instance)
        {
            writer.Put(instance.GroupId);
            writer.Put(instance.Type);
            _userDtoSerializer.Serialize(writer, instance.UserDto);
        }
    }
}
