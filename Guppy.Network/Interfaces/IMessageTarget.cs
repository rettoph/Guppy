using Guppy.Interfaces;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    /// <summary>
    /// Message targets are specific objects that can
    /// create, store, send, and parse network messages
    /// </summary>
    public interface IMessageTarget : IUniqueObject
    {
        /// <summary>
        /// Create and enqueue a new outbound message.
        /// All messaged created by this method should be
        /// send when this.Flush() is called.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        NetOutgoingMessage CreateMessage(String type, NetDeliveryMethod method = NetDeliveryMethod.UnreliableSequenced, Int32 sequenceChanel = 0, NetConnection target = null);

        /// <summary>
        /// Send all enqueued messages.
        /// </summary>
        void Flush();

        /// <summary>
        /// Handle a new incoming message recieved.
        /// </summary>
        /// <param name="im"></param>
        void HandleMessage(NetIncomingMessage im);

        /// <summary>
        /// Add a custom incoming message handler
        /// </summary>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        void AddMessageHandler(String type, Action<NetIncomingMessage> handler);
    }
}
