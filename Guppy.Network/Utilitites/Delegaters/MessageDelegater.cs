using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilitites.Delegaters
{
    public class MessageDelegater : Delegater<String, NetIncomingMessage>
    {
        public MessageDelegater(ILogger logger) : base(logger)
        {
        }
    }
}
