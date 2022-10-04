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

        public NetDatum Deserialize(NetDataReader reader)
        {
            var datum = _types[NetId.Byte.Read(reader)].Create();
            datum.Deserialize(reader);

            return datum;
        }

        public NetDatum<T> Serialize<T>(NetDataWriter writer, in T value)
        {
            var datum = _types[typeof(T)].Create();
            datum.Serialize(writer);

            return (NetDatum<T>)datum;
        }
    }
}
