using Guppy.Interfaces;
using Guppy.Network.Configurations;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    /// <summary>
    /// A target is an object that can
    /// recieve and create messages.
    /// </summary>
    public interface ITarget : IDriven
    {
        NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, int sequenceChanel = 0, NetConnection recipient = null);
        void SendMessage(NetOutgoingMessageConfiguration om);
    }
}
