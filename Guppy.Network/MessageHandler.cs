using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class MessageHandler : Dictionary<String, Action<NetIncomingMessage>>
    {
    }
}
