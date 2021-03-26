using Guppy.Network.Contexts;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    /// <summary>
    /// An object capable of creating & sending messages.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Create & enque a new <see cref="NetOutgoingMessage"/> instance.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        NetOutgoingMessage CreateMessage(NetOutgoingMessageContext context);
    }
}
