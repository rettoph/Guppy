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
        public readonly NetMessenger Messenger;

        public abstract IEnumerable<NetSerialized> Appendages { get; }
        public abstract IEnumerable<NetPeer> Recipients { get; }

        protected NetOutgoingMessage(NetMessenger mesenger)
        {
            this.Messenger = mesenger;
        }

        public abstract NetOutgoingMessage Append<TAppendage>(in TAppendage appendage);
        public abstract NetOutgoingMessage AddRecipient(NetPeer recipient);
        public abstract NetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients);

        public abstract NetOutgoingMessage EnqueueSend();

        /// <summary>
        /// Send the message immidiately.
        /// </summary>
        public abstract NetOutgoingMessage Send();

        /// <summary>
        /// Enqueue the message to be sent once the target <see cref="Room"/>'s
        /// <see cref="Services.RoomMessageService.SendEnqueued"/>
        /// is called.
        /// </summary>
        public abstract NetOutgoingMessage Recycle();
    }

    public sealed class NetOutgoingMessage<T> : NetOutgoingMessage
    {
        private readonly Buffer<NetPeer> _recipients;
        private readonly NetDataWriter _writer;
        private readonly List<NetSerialized> _appendages;
        private readonly INetSerializerProvider _serializers;
        private readonly NetSerializer<T> _serializer;

        public readonly new NetMessenger<T> Messenger;

        public Room Room = default!;
        public NetSerialized<T> Content = default!;

        public override IEnumerable<NetSerialized> Appendages => _appendages;

        public override IEnumerable<NetPeer> Recipients => _recipients;

        public NetOutgoingMessage(
            INetSerializerProvider serializers,
            NetSerializer<T> serializer,
            int recipientsBufferSize,
            NetMessenger<T> messenger) : base(messenger)
        {
            _serializers = serializers;
            _serializer = serializer;
            _writer = new NetDataWriter();
            _appendages = new List<NetSerialized>();
            _recipients = new Buffer<NetPeer>(recipientsBufferSize);

            this.Messenger = messenger;

            _writer.Put(this.Messenger.Id.Bytes);
        }

        internal void Write(Room room, in T content)
        {
            _writer.Put(room.Id);

            this.Room = room;
            this.Content = _serializer.Serialize(in content, _writer);
        }

        public override NetOutgoingMessage<T> Append<TAppendage>(in TAppendage appendage)
        {
            NetSerialized serialized = _serializers.Serialize(in appendage, _writer);
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

            _writer.SetPosition(this.Messenger.Id.Bytes.Length);

            this.Messenger.TryRecycle(this);

            return this;
        }

        public override NetOutgoingMessage<T> Send()
        {
            foreach(NetPeer recipient in _recipients)
            {
                recipient.Send(
                    _writer, 
                    this.Messenger.OutgoingChannel, 
                    this.Messenger.DeliveryMethod);
            }

            return this;
        }

        public override NetOutgoingMessage<T> EnqueueSend()
        {
            this.Room.Messages.EnqueueOutgoing(this);

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
    }
}
