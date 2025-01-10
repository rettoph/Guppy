using Guppy.Core.Network.Common.Dtos;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Messages;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Serialization.NetSerializers
{
    internal sealed class ConnectionRequestResponseNetSerializer : NetSerializer<ConnectionRequestResponse>
    {
        private INetSerializer<UserDto> _userDtoSerializer = null!;

        public override void Initialize(INetSerializerService serializers)
        {
            base.Initialize(serializers);

            this._userDtoSerializer = serializers.Get<UserDto>();
        }

        public override ConnectionRequestResponse Deserialize(NetDataReader reader)
        {
            ConnectionRequestResponseType type = reader.GetEnum<ConnectionRequestResponseType>();
            UserDto? systemUser = type == ConnectionRequestResponseType.Accepted ? this._userDtoSerializer.Deserialize(reader) : null;
            UserDto? currentUser = type == ConnectionRequestResponseType.Accepted ? this._userDtoSerializer.Deserialize(reader) : null;

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
                this._userDtoSerializer.Serialize(writer, instance.SystemUser!);
                this._userDtoSerializer.Serialize(writer, instance.CurrentUser!);
            }
        }
    }
}