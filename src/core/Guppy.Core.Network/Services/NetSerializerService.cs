using Guppy.Core.Common.Collections;
using Guppy.Core.Network.Common.Serialization;
using System.Collections;

namespace Guppy.Core.Network.Common.Services
{
    internal sealed class NetSerializerService : INetSerializerService
    {
        private DoubleDictionary<INetId, Type, INetSerializer> _serializers;

        public NetSerializerService(IEnumerable<INetSerializer> serializers)
        {
            _serializers = new DoubleDictionary<INetId, Type, INetSerializer>(serializers.Count());

            byte id = 0;
            foreach (var serializer in serializers)
            {
                if (_serializers.TryAdd(NetId.Create(id), serializer.Type, serializer))
                {
                    serializer.Id = NetId.Create(id);
                    id += 1;
                }
            }

            foreach (var serializer in _serializers.Values)
            {
                serializer.Initialize(this);
            }
        }

        public INetSerializer<T> Get<T>() where T : notnull
        {
            try
            {
                return (INetSerializer<T>)_serializers[typeof(T)];
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
                return _serializers[type];
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
                return _serializers[id];
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException($"{nameof(NetSerializerService)}::{nameof(Get)} - No {nameof(INetSerializer)} registered for id {id.Value}", e);
            }
        }

        public IEnumerator<INetSerializer> GetEnumerator()
        {
            return _serializers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
