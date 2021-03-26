using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Contexts;
using Lidgren.Network;
using Guppy.Network.Interfaces;

namespace Guppy.Network.Utilities
{
    /// <summary>
    /// Class used to contain a <see cref="NetOutgoingMessageContext"/>,
    /// <see cref="NetOutgoingMessage"/>, and 0 to many <see cref="IUser"/>
    /// instances to broadcast the message to.
    /// </summary>
    internal sealed class NetOutgoingMessageContainer
    {
        #region Public Fields
        public IPipe Pipe;
        public NetConnection Recipient;
        public NetOutgoingMessageContext Context;
        public NetOutgoingMessage Message;
        public Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> Filter;
        #endregion

        #region Constructors
        public NetOutgoingMessageContainer(
            IPipe pipe,
            NetOutgoingMessageContext context, 
            NetOutgoingMessage message,
            NetConnection recipient,
            Func<IEnumerable<NetConnection>, IEnumerable<NetConnection>> filter)
        {
            this.Pipe = pipe;
            this.Context = context;
            this.Message = message;
            this.Recipient = recipient;
            this.Filter = filter;
        }
        #endregion
    }
}
