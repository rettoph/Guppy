using Guppy.Interfaces;
using Guppy.Network.Utilities.DynamicDelegaters;
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
        /// The default message delegater used to handle incoming messages.
        /// </summary>
        MessageDelegater Messages { get; }

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
    }
}
