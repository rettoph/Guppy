﻿using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network
{
    internal sealed class NetIncomingMessage<T> : INetIncomingMessage<T>
        where T : notnull
    {
        private readonly IPeer _peer;
        private readonly INetSerializerService _serializers;
        private readonly INetSerializer<T> _serializer;

        public Sender Sender { get; private set; }

        public T Body { get; private set; }

        public byte Channel { get; private set; }

        public DeliveryMethod DeliveryMethod { get; private set; }

        public INetMessageType<T> Type { get; private set; }

        public INetGroup Group { get; private set; }

        object INetIncomingMessage.Body => this.Body;
        ISender INetIncomingMessage.Sender => this.Sender;
        INetMessageType INetIncomingMessage.Type => this.Type;

        internal NetIncomingMessage(
            IPeer peer,
            INetSerializerService serializers,
            INetMessageType<T> type)
        {
            this._peer = peer;
            this._serializers = serializers;
            this._serializer = this._serializers.Get<T>();

            this.Sender = new Sender(this._peer.Users);
            this.Body = default!;
            this.Group = default!;
            this.Type = type;
        }

        public void Read(NetPeer sender, NetDataReader reader, ref byte channel, ref DeliveryMethod deliveryMethod)
        {
            byte groupId = reader.GetByte();
            this.Group = this._peer.Groups.GetById(groupId);
            this.Body = this._serializer.Deserialize(reader);
            this.Channel = channel;
            this.DeliveryMethod = deliveryMethod;
            this.Sender.Peer = sender;
        }

        public void Recycle()
        {
            this.Type.Recycle(this);
        }

        public INetIncomingMessage<T> Publish()
        {
            this.Group.Publish(this);

            return this;
        }

        INetIncomingMessage INetIncomingMessage.Publish()
        {
            return this.Publish();
        }

        void IDisposable.Dispose()
        {
            this.Recycle();
        }

        void IMessage.Publish(IMessageBus messageBus)
        {
            messageBus.Publish<SubscriberSequenceGroupEnum, INetIncomingMessage<T>>(this);
        }
    }
}