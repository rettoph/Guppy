using Guppy.Network;
using Guppy.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading
{
    public static class IBusMessageCollectionExtensions
    {
        public static IBusMessageCollection AddNetIncomingMessage<T>(this IBusMessageCollection bus, int queue)
        {
            return bus.Add<NetIncomingMessage<T>>(queue);
        }

        public static IBusMessageCollection AddNetDeserialized<T>(this IBusMessageCollection bus, int queue)
        {
            return bus.Add<NetDeserialized<T>>(queue);
        }
    }
}
