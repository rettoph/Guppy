using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Providers
{
    public interface IRoomProvider
    {
        Room Get(in byte id);

        void ProcessIncoming(NetIncomingMessage message);
    }
}
