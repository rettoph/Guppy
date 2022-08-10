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
        NetDatum<T> Serialize<T>(NetDataWriter writer, in T value);

        NetDatum Deserialize(NetDataReader reader);
    }
}
