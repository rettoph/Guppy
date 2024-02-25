using Guppy.Messaging;
using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using System.Collections.ObjectModel;

namespace Guppy.Network
{
    internal sealed class NetOutgoingMessage<T> : INetOutgoingMessage<T>
        where T : notnull
    {
        private readonly INetScope _scope;
        private readonly INetSerializer<T> _serializer;
        private readonly INetSerializerProvider _serializers;
        private readonly List<NetPeer> _recipients;


        object INetOutgoingMessage.Body => this.Body;
        NetMessageType INetOutgoingMessage.Type => this.Type;
        public T Body { get; private set; }
        public byte OutgoingChannel { get; private set; }
        public DeliveryMethod DeliveryMethod { get; private set; }
        public NetMessageType<T> Type { get; }
        public NetDataWriter Writer { get; }

        Type IMessage.Type { get; } = typeof(INetOutgoingMessage<T>);

        public IReadOnlyList<NetPeer> Recipients { get; }

        internal NetOutgoingMessage(
            NetMessageType<T> type,
            INetScope scope,
            INetSerializerProvider serializers)
        {
            _scope = scope;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();
            _recipients = new List<NetPeer>();

            this.Body = default!;
            this.Type = type;
            this.OutgoingChannel = this.Type.DefaultOutgoingChannel;
            this.DeliveryMethod = this.Type.DefaultDeliveryMethod;
            this.Writer = new NetDataWriter();


            this.Recipients = new ReadOnlyCollection<NetPeer>(_recipients);

            this.Writer.Put(_scope.Id);
            this.Writer.Put(this.Type.Id);
        }

        public void Write(in T body)
        {
            this.Body = body;

            _serializer.Serialize(this.Writer, in body);
        }

        public void Recycle()
        {
            this.Writer.SetPosition(1 + NetId.Byte.SizeInBytes);

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
            _scope.Send(this);

            return this;
        }

        public INetOutgoingMessage<T> Enqueue()
        {
            _scope.Enqueue(this);

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

        INetOutgoingMessage INetOutgoingMessage.Enqueue()
        {
            return this.Enqueue();
        }
    }
}
