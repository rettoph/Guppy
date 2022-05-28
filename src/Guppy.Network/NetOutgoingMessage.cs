using Guppy.Network.Components;
using Guppy.Network.Constants;
using Guppy.Network.Providers;
using Guppy.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using Minnow.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetOutgoingMessage
    {
        public readonly NetMessageFactory Factory;

        public abstract IEnumerable<NetSerialized> Appendages { get; }
        public abstract IEnumerable<NetPeer> Recipients { get; }

        protected NetOutgoingMessage(NetMessageFactory factory)
        {
            this.Factory = factory;
        }

        public abstract NetOutgoingMessage Append<TAppendage>(in TAppendage appendage);
        public abstract NetOutgoingMessage AddRecipient(NetPeer recipient);
        public abstract NetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients);

        public abstract NetOutgoingMessage Send();
        public abstract NetOutgoingMessage Enqueue();
        public abstract NetOutgoingMessage Recycle();
    }

    public sealed class NetOutgoingMessage<T> : NetOutgoingMessage
    {
        private readonly Buffer<NetPeer> _recipients;
        private readonly List<NetSerialized> _appendages;
        private readonly INetSerializerProvider _serializers;
        private readonly NetSerializer<T> _serializer;

        public readonly new NetMessageFactory<T> Factory;
        public readonly NetDataWriter Writer;

        public NetMessenger? Messenger;
        public NetSerialized<T> Content = null!;

        public override IEnumerable<NetSerialized> Appendages => _appendages;

        public override IEnumerable<NetPeer> Recipients => _recipients;

        public NetOutgoingMessage(
            INetSerializerProvider serializers,
            NetSerializer<T> serializer,
            int recipientsBufferSize,
            NetMessageFactory<T> messenger) : base(messenger)
        {
            _serializers = serializers;
            _serializer = serializer;
            _appendages = new List<NetSerialized>();
            _recipients = new Buffer<NetPeer>(recipientsBufferSize);

            this.Factory = messenger;
            this.Writer = new NetDataWriter();

            this.Writer.Put(this.Factory.Id.Bytes);
        }

        internal void Write(NetMessenger messenger, in T content)
        {
            this.Messenger = messenger;

            this.Writer.Put(this.Messenger.Scope.Id);
            this.Writer.Put(this.Messenger.Id);

            this.Content = _serializer.Serialize(in content, this.Writer);
        }

        public override NetOutgoingMessage<T> Append<TAppendage>(in TAppendage appendage)
        {
            NetSerialized serialized = _serializers.Serialize(in appendage, this.Writer);
            _appendages.Add(serialized);

            return this;
        }

        /// <summary>
        /// Recycle the outgoing message so that it can be entirely reused.
        /// </summary>
        public override NetOutgoingMessage<T> Recycle()
        {
            this.Content.Recycle();

            foreach(NetSerialized appendage in _appendages)
            {
                appendage.Recycle();
            }

            _appendages.Clear();
            _recipients.Reset();

            this.Writer.SetPosition(this.Factory.Id.Bytes.Length);
            this.Factory.TryRecycle(this);

            return this;
        }

        public override NetOutgoingMessage<T> AddRecipient(NetPeer recipient)
        {
            _recipients.Add(recipient);

            return this;
        }

        public override NetOutgoingMessage<T> AddRecipients(IEnumerable<NetPeer> recipients)
        {
            _recipients.AddRange(recipients);

            return this;
        }

        public override NetOutgoingMessage<T> Send()
        {
            foreach(NetPeer recipient in _recipients)
            {
                recipient.Send(this.Writer, this.Factory.OutgoingChannel, this.Factory.DeliveryMethod);
            }

            return this;
        }

        public override NetOutgoingMessage<T> Enqueue()
        {
            this.Messenger!.Scope.Enqueue(this);

            return this;
        }
    }
}
