using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Delegates
{
    public delegate void NetSerializeDelegate<T>(NetDataWriter writer, INetDatumProvider datum, in T instance);
}
