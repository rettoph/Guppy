using Guppy.Common;
using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    internal sealed class NetOutgoingMessage<T> : INetOutgoingMessage<T>
        where T : notnull
    {
        private readonly NetScope _scope;
        private readonly NetDataWriter _writer;
        private readonly NetMessageType<T> _type;
        private readonly INetSerializer<T> _serializer;
        private readonly INetSerializerProvider _serializers;
        private readonly List<object> _data;
        private readonly List<NetPeer> _recipients;
        private T _body;
        private byte _outgoingChannel;
        private DeliveryMethod _deliveryMethod;


        object INetOutgoingMessage.Body => _body;
        NetMessageType INetOutgoingMessage.Type => _type;
        public T Body => _body;
        public byte OutgoingChannel => _outgoingChannel;
        public DeliveryMethod DeliveryMethod => _deliveryMethod;
        public NetMessageType<T> Type => _type;
        public IEnumerable<object> Data => _data;
        public NetDataWriter Writer => _writer;

        Type IMessage.Type { get; } = typeof(INetOutgoingMessage<T>);

        internal NetOutgoingMessage(
            NetMessageType<T> type,
            NetScope scope,
            INetSerializerProvider serializers)
        {
            _body = default!;
            _scope = scope;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();

            _type = type;

            _data = new List<object>();
            _recipients = new List<NetPeer>();
            _writer = new NetDataWriter();

            _outgoingChannel = this.Type.DefaultOutgoingChannel;
            _deliveryMethod = this.Type.DefaultDeliveryMethod;

            _writer.Put(_scope.id);
            _type.Id.Write(_writer);
        }

        public void Write(in T body)
        {
            _body = body;

            _serializer.Serialize(_writer, in body);
        }

        public void Recycle()
        {
            _writer.SetPosition(1 + NetId.Byte.SizeInBytes);

            _data.Clear();
            _recipients.Clear();

            _outgoingChannel = this.Type.DefaultOutgoingChannel;
            _deliveryMethod = this.Type.DefaultDeliveryMethod;

            this.Type.Recycle(this);
        }

        public INetOutgoingMessage<T> Append<TData>(in TData value)
            where TData : notnull
        {
            _serializers.Serialize(_writer, true, in value);
            _data.Add(value);

            return this;
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
            _outgoingChannel = outgoingChannel;

            return this;
        }

        public INetOutgoingMessage<T> SetDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            _deliveryMethod = deliveryMethod;

            return this;
        }

        public INetOutgoingMessage<T> Send()
        {
            foreach (NetPeer recipient in _recipients)
            {
                recipient.Send(_writer, _outgoingChannel, _deliveryMethod);
            }

            return this;
        }

        public INetOutgoingMessage<T> Enqueue()
        {
            _scope.Bus.Publish(this);

            return this;
        }

        public void Dispose()
        {
            this.Recycle();
        }

        INetOutgoingMessage INetOutgoingMessage.Append<TData>(in TData value)
        {
            return this.Append(in value);
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
