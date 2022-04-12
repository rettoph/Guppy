using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetIncomingMessage
    {
        internal abstract void Read(NetDataReader reader);
        public abstract void Recycle();
    }

    public sealed class NetIncomingMessage<T> : NetIncomingMessage
    {
        private List<NetDeserialized> _appendages;
        private INetSerializerProvider _serializers;
        private NetSerializer<T> _serializer;

        public readonly NetMessenger<T> Messenger;

        public byte RoomId;
        public NetDeserialized<T> Content = default!;

        public NetIncomingMessage(
            INetSerializerProvider serializers,
            NetSerializer<T> serializer,
            NetMessenger<T> messenger)
        {
            _serializers = serializers;
            _serializer = serializer;
            _appendages = new List<NetDeserialized>();

            this.Messenger = messenger;
        }

        public IEnumerable<NetDeserialized> Appendages => _appendages;

        internal override void Read(NetDataReader reader)
        {
            this.RoomId = reader.GetByte();
            this.Content = _serializer.Deserialize(reader);

            while (!reader.EndOfData)
            {
                _appendages.Add(_serializers.Deserialize(reader));
            }
        }

        public override void Recycle()
        {
            this.Content.Recycle();

            foreach (NetDeserialized appendage in _appendages)
            {
                appendage.Recycle();
            }

            _appendages.Clear();

            this.Messenger.TryRecycle(this);
        }
    }
}
