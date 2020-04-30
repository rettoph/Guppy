using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Lidgren.Network;

namespace Guppy.Network.Peers
{
    public sealed class Client : Peer
    {
        #region Peer Implementation
        protected override NetPeer GetPeer(ServiceProvider provider)
        {
            return provider.GetService<NetClient>();
        }
        #endregion
    }
}
