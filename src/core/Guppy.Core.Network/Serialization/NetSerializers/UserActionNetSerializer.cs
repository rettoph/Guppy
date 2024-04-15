using Guppy.Core.Common.Attributes;
using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Messages;
using Guppy.Core.Network.Common.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Serialization.NetSerializers
{
    [AutoLoad]
    internal sealed class UserActionNetSerializer : NetSerializer<UserAction>
    {
        private INetSerializer<UserDto> _userDtoSerializer;

        public override void Initialize(INetSerializerService serializers)
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
