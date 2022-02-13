using LiteNetLib.Utils;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    internal sealed class NetDataWriterFactory : ConcurrentFactory<NetDataWriter>
    {
        public NetDataWriterFactory() : base(() => new NetDataWriter(), 100)
        {
        }

        public override bool TryReturnToPool(NetDataWriter instance)
        {
            instance.Reset();
            return base.TryReturnToPool(instance);
        }
    }
}
