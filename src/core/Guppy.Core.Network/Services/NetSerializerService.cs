using System.Collections;
using Guppy.Core.Common.Collections;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Serialization;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Services
{
    internal sealed class NetSerializerService : INetSerializerService
    {
        private readonly DoubleDictionary<INetId, Type, INetSerializer> _serializers;

        public NetSerializerService(IEnumerable<INetSerializer> serializers)
        {
            this._serializers = new DoubleDictionary<INetId, Type, INetSerializer>(serializers.Count());

            byte id = 0;
            foreach (var serializer in serializers)
            {
                if (this._serializers.TryAdd(NetId.Create(id), serializer.Type, serializer))
                {
                    serializer.Id = NetId.Create(id);
                    id += 1;
                }
            }

            foreach (var serializer in this._serializers.Values)
            {
                serializer.Initialize(this);
            }
        }

        public INetSerializer<T> Get<T>() where T : notnull
        {
            try
            {
                return (INetSerializer<T>)this._serializers[typeof(T)];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"{nameof(NetSerializerService)}::{nameof(Get)} - No {nameof(INetSerializer)} registered for type {typeof(T).Name}", e);
            }
        }

        public INetSerializer Get(Type type)
        {
            try
            {
                return this._serializers[type];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"{nameof(NetSerializerService)}::{nameof(Get)} - No {nameof(INetSerializer)} registered for type {type.Name}", e);
            }
        }

        public INetSerializer Get(INetId id)
        {
            try
            {
                return this._serializers[id];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"{nameof(NetSerializerService)}::{nameof(Get)} - No {nameof(INetSerializer)} registered for id {id.Value}", e);
            }
        }

        public IEnumerator<INetSerializer> GetEnumerator()
        {
            return this._serializers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}