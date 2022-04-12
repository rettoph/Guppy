using Guppy.Network.Delegates;
using Guppy.Network.Structs;
using LiteNetLib.Utils;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetSerializer
    {
        public readonly Type Type;
        public readonly DynamicId Id;

        protected NetSerializer(Type type, DynamicId id)
        {
            this.Type = type;
            this.Id = id;
        }

        public abstract NetDeserialized Deserialize(NetDataReader reader);
    }

    public sealed class NetSerializer<T> : NetSerializer
    {
        private NetSerializeDelegate<T> _serialize;
        private NetDeserializeDelegate<T> _deserialize;

        private Factory<NetDeserialized<T>> _deserializedFactory;
        private Factory<NetSerialized<T>> _serializedFactory;

        public NetSerializer(DynamicId id, NetSerializeDelegate<T> serialize, NetDeserializeDelegate<T> deserialize) : base(typeof(T), id)
        {
            _serialize = serialize;
            _deserialize = deserialize;

            _deserializedFactory = new Factory<NetDeserialized<T>>(() => new NetDeserialized<T>(this));
            _serializedFactory = new Factory<NetSerialized<T>>(() => new NetSerialized<T>(this));
        }

        public override NetDeserialized<T> Deserialize(NetDataReader reader)
        {
            NetDeserialized<T> deserialized = _deserializedFactory.GetInstance();
            _deserialize(reader, out deserialized.Instance);

            return deserialized;
        }

        public NetSerialized<T> Serialize(in T instance, NetDataWriter writer)
        {
            NetSerialized<T> serialized = _serializedFactory.GetInstance();
            _serialize(writer, in instance);

            return serialized;
        }

        internal void TryRecycle(NetDeserialized<T> deserialized)
        {
            _deserializedFactory.TryReturnToPool(deserialized);
        }

        internal void TryRecycle(NetSerialized<T> serialized)
        {
            _serializedFactory.TryReturnToPool(serialized);
        }
    }
}
