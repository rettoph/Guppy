﻿using Guppy.Core.Network.Common.Peers;

namespace Guppy.Core.Network.Common.Groups
{
    internal sealed class ClientNetGroup : BaseNetGroup
    {
        public ClientNetGroup(byte id, IPeer peer) : base(id, peer)
        {
        }
    }
}
