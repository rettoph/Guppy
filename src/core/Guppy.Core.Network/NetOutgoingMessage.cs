using System.Collections.ObjectModel;
using Guppy.Core.Messaging.Common;
using Guppy.Core.Messaging.Common.Enums;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Peers;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network
{
    internal sealed class NetOutgoingMessage<T> : INetOutgoingMessage<T>
        where T : notnull
    {
        private readonly Peer _peer;
        private readonly INetSerializer<T> _serializer;
        private readonly INetSerializerService _serializers;
        private readonly List<NetPeer> _recipients;


        object INetOutgoingMessage.Body => this.Body;
        INetMessageType INetOutgoingMessage.Type => this.Type;
        public T Body { get; private set; }
        public byte OutgoingChannel { get; private set; }
        public DeliveryMethod DeliveryMethod { get; private set; }
        public INetGroup Group { get; private set; }
        public INetMessageType<T> Type { get; }
        public NetDataWriter Writer { get; }

        public IReadOnlyList<NetPeer> Recipients { get; }

        internal NetOutgoingMessage(
            Peer peer,
            INetSerializerService serializers,
            NetMessageType<T> type)
        {
            this._peer = peer;
            this._serializers = serializers;
            this._serializer = this._serializers.Get<T>();
            this._recipients = [];

            this.Body = default!;
            this.Group = default!;
            this.Type = type;
            this.OutgoingChannel = this.Type.DefaultOutgoingChannel;
            this.DeliveryMethod = this.Type.DefaultDeliveryMethod;
            this.Writer = new NetDataWriter();


            this.Recipients = new ReadOnlyCollection<NetPeer>(this._recipients);

            this.Writer.Put(this.Type.Id);
        }

        public void Write(in INetGroup group, in T body)
        {
            this.Group = group;
            this.Writer.Put(group.Id);

            this.Body = body;
            this._serializer.Serialize(this.Writer, in body);
        }

        public void Recycle()
        {
            this.Writer.SetPosition(sizeof(byte) * 1); // The message type byte

            this._recipients.Clear();

            this.OutgoingChannel = this.Type.DefaultOutgoingChannel;
            this.DeliveryMethod = this.Type.DefaultDeliveryMethod;

            this.Type.Recycle(this);
        }

        public INetOutgoingMessage<T> AddRecipient(NetPeer recipient)
        {
            this._recipients.Add(recipient);

            return this;
        }

        public INetOutgoingMessage<T> AddRecipients(IEnumerable<NetPeer> recipients)
        {
            this._recipients.AddRange(recipients);

            return this;
        }

        public INetOutgoingMessage<T> SetOutgoingChannel(byte outgoingChannel)
        {
            this.OutgoingChannel = outgoingChannel;

            return this;
        }

        public INetOutgoingMessage<T> SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            this.DeliveryMethod = deliveryMethod;

            return this;
        }

        public INetOutgoingMessage<T> Send()
        {
            this._peer.Send(this);

            return this;
        }

        public void Dispose()
        {
            this.Recycle();
        }

        INetOutgoingMessage INetOutgoingMessage.AddRecipient(NetPeer recipient)
        {
            return this.AddRecipient(recipient);
        }

        INetOutgoingMessage INetOutgoingMessage.AddRecipients(IEnumerable<NetPeer> recipients)
        {
            return this.AddRecipients(recipients);
        }

        INetOutgoingMessage INetOutgoingMessage.SetOutgoingChannel(byte outgoingChannel)
        {
            return this.SetOutgoingChannel(outgoingChannel);
        }

        INetOutgoingMessage INetOutgoingMessage.SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            return this.SetDeliveryMethod(deliveryMethod);
        }

        INetOutgoingMessage INetOutgoingMessage.Send()
        {
            return this.Send();
        }

        void IMessage.Publish(IMessageBus messageBus)
        {
            messageBus.Publish<SubscriberSequenceGroupEnum, INetOutgoingMessage<T>>(this);
        }
    }
}