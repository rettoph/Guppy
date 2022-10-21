using Guppy.Common.Collections;
using Guppy.Network.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetSerializerProvider : INetSerializerProvider
    {
        private DoubleDictionary<INetId, Type, INetSerializer> _serializers;

        public NetSerializerProvider(IEnumerable<INetSerializer> serializers)
        {
            _serializers = new DoubleDictionary<INetId, Type, INetSerializer>(serializers.Count());

            byte id = 0;
            foreach(var serializer in serializers)
            {
                if(_serializers.TryAdd(NetId.Create(id), serializer.Type, serializer))
                {
                    serializer.Id = NetId.Create(id);
                    id += 1;
                }
            }

            foreach(var serializer in _serializers.Values)
            {
                serializer.Initialize(this);
            }
        }

        public INetSerializer<T> Get<T>() where T : notnull
        {
            return (INetSerializer<T>)_serializers[typeof(T)];
        }

        public INetSerializer Get(Type type)
        {
            return _serializers[type];
        }

        public INetSerializer Get(INetId id)
        {
            return _serializers[id];
        }

        public IEnumerator<INetSerializer> GetEnumerator()
        {
            return _serializers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
