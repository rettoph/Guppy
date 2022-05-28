using Guppy.EntityComponent;
using Guppy.Network.Providers;
using Guppy.Threading;
using LiteNetLib;
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
        public ushort MessengerId;
        public NetPeer? Sender;

        public abstract IEnumerable<NetDeserialized> Appendages { get; }

        public abstract IEnumerable<NetDeserialized> Data { get; }

        internal abstract void Read(NetPeer sender, NetDataReader reader);

        public abstract void Recycle();
    }

    public sealed class NetIncomingMessage<T> : NetIncomingMessage
    {
        private readonly List<NetDeserialized> _appendages;
        private readonly INetSerializerProvider _serializers;
        private readonly NetSerializer<T> _serializer;

        public readonly NetMessageFactory<T> Factory;

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
            NetMessageFactory<T> messenger)
        {
            _serializers = serializers;
            _serializer = serializer;
            _appendages = new List<NetDeserialized>();

            this.Factory = messenger;
        }

        internal override void Read(NetPeer sender, NetDataReader reader)
        {
            this.ScopeId = reader.GetByte();
            this.MessengerId = reader.GetUShort();
            this.Sender = sender;
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

            this.Factory.TryRecycle(this);
        }
    }
}
