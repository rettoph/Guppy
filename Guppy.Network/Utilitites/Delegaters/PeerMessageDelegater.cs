﻿using Guppy.Extensions.Collection;
using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Utilitites.Delegaters
{
    public class PeerMessageDelegater : Delegater<NetIncomingMessageType, NetIncomingMessage>
    {
        private static NetIncomingMessageType[] NetIncomingMessageTypes = (NetIncomingMessageType[])Enum.GetValues(typeof(NetIncomingMessageType));

        public PeerMessageDelegater(ILogger logger) : base(logger)
        {
            // Automatically register all NetIncomingMessageTypes
            PeerMessageDelegater.NetIncomingMessageTypes.ForEach(this.TryRegister);
        }
    }
}
