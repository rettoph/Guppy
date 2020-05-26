using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    /// <summary>
    /// Represents a service capable of transmitting
    /// itself through a network
    /// </summary>
    public interface INetworkService : IService
    {
        void TryRead(NetIncomingMessage im);
        void TryWrite(NetOutgoingMessage om);
    }
}
