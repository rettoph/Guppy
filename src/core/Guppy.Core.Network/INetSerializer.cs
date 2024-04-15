﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Network.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network
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
