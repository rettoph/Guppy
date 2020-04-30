using Guppy.DependencyInjection;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Peers
{
    public sealed class Server : Peer
    {
        #region Peer Implementation
        protected override NetPeer GetPeer(ServiceProvider provider)
        {
            return provider.GetService<NetServer>();
        }
        #endregion
    }
}
