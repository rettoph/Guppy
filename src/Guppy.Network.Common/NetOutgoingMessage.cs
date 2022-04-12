using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetOutgoingMessage
    {
        public abstract void Recycle();
    }

    public sealed class NetOutgoingMessage<T> : NetOutgoingMessage
    {
        private NetDataWriter _writer;
        private List<NetSerialized> _appendages;
        private INetSerializerProvider _serializers;
        private NetSerializer<T> _serializer;

        public readonly NetMessenger<T> Messenger;

        public byte RoomId;
        public NetSerialized<T> Content = default!;

        public IEnumerable<NetSerialized> Appendages => _appendages;

        public NetOutgoingMessage(
            INetSerializerProvider serializers,
            NetSerializer<T> serializer,
            NetMessenger<T> messenger)
        {
            _serializers = serializers;
            _serializer = serializer;
            _writer = new NetDataWriter();
            _appendages = new List<NetSerialized>();

            this.Messenger = messenger;

            _writer.Put(this.Messenger.Id.Bytes);
        }

        internal void Write(in byte roomId, in T content)
        {
            _writer.Put(roomId);

            this.RoomId = roomId;
            this.Content = _serializer.Serialize(in content, _writer);
        }

        public void Append<TAppendage>(in TAppendage appendage)
        {
            NetSerialized serialized = _serializers.Serialize(in appendage, _writer);
            _appendages.Add(serialized);
        }

        public override void Recycle()
        {
            this.Content.Recycle();

            foreach(NetSerialized appendage in _appendages)
            {
                appendage.Recycle();
            }

            _appendages.Clear();

            _writer.SetPosition(this.Messenger.Id.Bytes.Length);

            this.Messenger.TryRecycle(this);
        }
    }
}
