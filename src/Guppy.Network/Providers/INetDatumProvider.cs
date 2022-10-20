using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface INetDatumProvider
    {
        NetDatum<T> Serialize<T>(NetDataWriter writer, bool sign, in T value);
        NetDatum Serialize(NetDataWriter writer, bool sign, Type type, object value);

        NetDatum<T> Deserialize<T>(NetDataReader reader);
        NetDatum Deserialize(NetDataReader reader);
    }
}
