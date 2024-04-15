using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Network.Common.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Serialization
{
    [Service<INetSerializer>(ServiceLifetime.Singleton, true)]
    public interface INetSerializer
    {
        INetId Id { get; set; }

        Type Type { get; }

        void Initialize(INetSerializerService serializers);

        void Serialize(NetDataWriter writer, in object instance);

        object Deserialize(NetDataReader reader);
    }

    public interface INetSerializer<T> : INetSerializer
        where T : notnull
    {
        void Serialize(NetDataWriter writer, in T instance);
        new T Deserialize(NetDataReader reader);
    }
}
