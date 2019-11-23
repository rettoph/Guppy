using Guppy.Utilities.Delegaters;
using Lidgren.Network;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.Concurrent;
using System.Linq;
using Guppy.Network.Configurations;
using Guppy.Pooling.Interfaces;
using Guppy.Network.Groups;
using Guppy.Network.Extensions.Lidgren;
using xxHashSharp;

namespace Guppy.Network.Utilitites.Delegaters
{
    public class GroupMessageDelegater : MessageDelegater
    {
        #region Internal Attributes
        public Group Group { get; set; }
        #endregion

        #region Constructor
        public GroupMessageDelegater(IPool<NetOutgoingMessageConfiguration> outgoingMessagePool, NetPeer peer, ILogger logger) : base(outgoingMessagePool, peer, logger)
        {
        }
        #endregion

        #region MessageDelegatoer Implementation
        protected override bool CanSend()
        {
            return this.Group.connections.Any() && base.CanSend();
        }

        protected override void Send(NetPeer peer, NetOutgoingMessageConfiguration config)
        {
            if (config.Recipient == default(NetConnection))
                peer.SendMessage(config.Message, this.Group.connections, config.Method, config.SequenceChannel);
            else
                peer.SendMessage(config.Message, config.Recipient, config.Method, config.SequenceChannel);
        }

        protected override void Sign(NetOutgoingMessage om, string type)
        {
            om.Write(this.Group.Id);
            om.Write(xxHash.CalculateHash(Encoding.UTF8.GetBytes(type)));
        }
        #endregion
    }
}
