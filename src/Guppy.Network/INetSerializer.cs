using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public interface INetSerializer
    {
        INetId Id { get; internal set; }

        Type Type { get; }

        void Initialize(INetSerializerProvider provider);

        void Serialize(NetDataWriter writer, INetSerializerProvider serializers, in object instance);

        object Deserialize(NetDataReader reader, INetSerializerProvider serializers);
    }

    public interface INetSerializer<T> : INetSerializer
        where T : notnull
    {
        void Serialize(NetDataWriter writer, INetSerializerProvider serializers, in T instance);
        new T Deserialize(NetDataReader reader, INetSerializerProvider serializers);
    }
}
