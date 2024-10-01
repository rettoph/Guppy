using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;
using Guppy.Core.Network.Peers;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.ObjectModel;

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

        Type IMessage.Type { get; } = typeof(INetOutgoingMessage<T>);

        public IReadOnlyList<NetPeer> Recipients { get; }

        internal NetOutgoingMessage(
            Peer peer,
            INetSerializerService serializers,
            NetMessageType<T> type)
        {
            _peer = peer;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();
            _recipients = new List<NetPeer>();

            this.Body = default!;
            this.Group = default!;
            this.Type = type;
            this.OutgoingChannel = this.Type.DefaultOutgoingChannel;
            this.DeliveryMethod = this.Type.DefaultDeliveryMethod;
            this.Writer = new NetDataWriter();


            this.Recipients = new ReadOnlyCollection<NetPeer>(_recipients);

            this.Writer.Put(this.Type.Id);
        }

        public void Write(in INetGroup group, in T body)
        {
            this.Group = group;
            this.Writer.Put(group.Id);

            this.Body = body;
            _serializer.Serialize(this.Writer, in body);
        }

        public void Recycle()
        {
            this.Writer.SetPosition(sizeof(byte) * 1); // The message type byte

            _recipients.Clear();

            this.OutgoingChannel = this.Type.DefaultOutgoingChannel;
            this.DeliveryMethod = this.Type.DefaultDeliveryMethod;

            this.Type.Recycle(this);
        }

        public INetOutgoingMessage<T> AddRecipient(NetPeer recipient)
        {
            _recipients.Add(recipient);

            return this;
        }

        public INetOutgoingMessage<T> AddRecipients(IEnumerable<NetPeer> recipients)
        {
            _recipients.AddRange(recipients);

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
            _peer.Send(this);

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
    }
}
