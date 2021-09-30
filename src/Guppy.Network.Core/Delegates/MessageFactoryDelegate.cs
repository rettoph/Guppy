using Guppy.Network.Contexts;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Delegates
{
    public delegate void MessageFactoryDelegate(Action<NetOutgoingMessage> writer, NetOutgoingMessageContext context, IEnumerable<NetConnection> recipients);
}
