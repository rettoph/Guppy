﻿using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Network.Providers;
using LiteNetLib;

namespace Guppy.Network.Definitions
{
    [Service<NetMessageTypeDefinition>(ServiceLifetime.Singleton, true)]
    public abstract class NetMessageTypeDefinition
    {
        public abstract Type Body { get; }
        public abstract DeliveryMethod DefaultDeliveryMethod { get; }
        public abstract byte DefaultOutgoingChannel { get; }

        internal abstract NetMessageType Build(byte id, INetSerializerProvider serializers, INetScope netScope);
    }

    public abstract class NetMessageTypeDefinition<TBody> : NetMessageTypeDefinition
        where TBody : notnull
    {
        public override Type Body => typeof(TBody);

        internal override NetMessageType Build(byte id, INetSerializerProvider serializers, INetScope netScope)
        {
            return new NetMessageType<TBody>(id, this.Body, this.DefaultDeliveryMethod, this.DefaultOutgoingChannel, serializers, netScope);
        }
    }
}
