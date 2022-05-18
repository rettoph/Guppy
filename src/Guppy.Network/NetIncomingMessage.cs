using Guppy.Network.Providers;
using Guppy.Threading;
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
        public byte ScopeId;
        public ushort TargetNetId;

        public abstract IEnumerable<NetDeserialized> Appendages { get; }

        public abstract IEnumerable<NetDeserialized> Data { get; }

        internal abstract void Read(NetDataReader reader);

        public abstract void Recycle();
    }

    public sealed class NetIncomingMessage<T> : NetIncomingMessage
    {
        private readonly List<NetDeserialized> _appendages;
        private readonly INetSerializerProvider _serializers;
        private readonly NetSerializer<T> _serializer;

        public readonly NetMessenger<T> Messenger;

        public NetDeserialized<T> Content = null!;

        public override IEnumerable<NetDeserialized> Appendages => _appendages;
        public override IEnumerable<NetDeserialized> Data
        {
            get
            {
                yield return this.Content;

                foreach(NetDeserialized appendage in _appendages)
                {
                    yield return appendage;
                }
            }
        }

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

        internal override void Read(NetDataReader reader)
        {
            this.ScopeId = reader.GetByte();
            this.TargetNetId = reader.GetUShort();
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
