using Guppy.Attributes;
using Guppy.Network.Providers;
using LiteNetLib.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Network
{
    [Service<INetSerializer>(ServiceLifetime.Singleton, true)]
    public interface INetSerializer
    {
        INetId Id { get; internal set; }

        Type Type { get; }

        void Initialize(INetSerializerProvider serializers);

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
