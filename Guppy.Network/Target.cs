using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    /// <summary>
    /// A target represents something that can create and recieve
    /// messages. By default, the only targets are groups and peers.
    /// </summary>
    public abstract class Target : Frameable
    {
        public abstract NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChanel = 0, NetConnection recipient = null);
    }
}
