using Guppy.Utilities.DynamicDelegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilities.DynamicDelegaters
{
    public class ActionDelegater : DynamicDelegater<String, NetIncomingMessage>
    {
        private NetworkEntity _parent;

        public ActionDelegater(NetworkEntity parent, ILogger logger) : base(logger)
        {
            _parent = parent;
        }

        public void HandleMessage(NetIncomingMessage im)
        {
            String type = im.ReadString();

            if (!this.TryInvoke(type, im))
                this.logger.LogWarning($"Unhandled network action => Type: {_parent.GetType().Name}, Action: {type}, Entity: {_parent.Id}");
        }
    }
}
