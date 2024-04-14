using Guppy.Engine.Attributes;
using Guppy.Engine.Enums;
using Guppy.Network.Services;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    [Service<INetSerializer>(ServiceLifetime.Singleton, true)]
    public interface INetSerializer
    {
        INetId Id { get; internal set; }

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
