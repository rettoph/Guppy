using Guppy.Common.Collections;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    internal sealed class NetDatumProvider : INetDatumProvider
    {
        private DoubleDictionary<INetId, Type, NetDatumType> _types;

        public NetDatumProvider(INetSerializerProvider serializers)
        {
            _types = serializers.BuildDatumTypes().ToDoubleDictionary(x => x.Serializer.Id, x => x.Serializer.Type);
        }

        /// <summary>
        /// Deserialize unsigned data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public NetDatum<T> Deserialize<T>(NetDataReader reader)
        {
            var datum = _types[typeof(T)].Create();
            datum.Deserialize(reader, this);

            return (NetDatum<T>)datum;
        }

        /// <summary>
        /// Deserialize signed data
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public NetDatum Deserialize(NetDataReader reader)
        {
            var datum = _types[NetId.Byte.Read(reader)].Create();
            datum.Deserialize(reader, this);

            return datum;
        }

        public NetDatum<T> Serialize<T>(NetDataWriter writer, bool sign, in T value)
        {
            var datum = (NetDatum<T>)_types[typeof(T)].Create();
            datum.Serialize(writer, this, sign, in value);

            return datum;
        }

        public NetDatum Serialize(NetDataWriter writer, bool sign, Type type, object value)
        {
            var datum = _types[value.GetType()].Create();
            datum.Serialize(writer, this, sign, in value);

            return datum;
        }
    }
}
