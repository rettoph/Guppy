using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetSerializerProvider
    {
        NetDeserialized Deserialize(NetDataReader reader);
        NetSerialized<T> Serialize<T>(in T instance, NetDataWriter writer);

        bool TryGetSerializer<T>(out NetSerializer<T>? serializer);
        bool TryGetSerializer(ushort id, out NetSerializer? serializer);
    }
}
