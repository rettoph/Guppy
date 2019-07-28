using Guppy.Utilities.DynamicDelegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities.DynamicDelegaters
{
    public class MessageDelegater : DynamicDelegater<String, NetIncomingMessage>
    {
        public MessageDelegater(ILogger logger) : base(logger)
        {
        }

        public void HandleMessage(NetIncomingMessage im)
        {
            String type = im.ReadString();

            if(!this.TryInvoke(type, im))
                this.logger.LogWarning($"Unhandled network message recieved => '{type}'");
        }
    }
}
