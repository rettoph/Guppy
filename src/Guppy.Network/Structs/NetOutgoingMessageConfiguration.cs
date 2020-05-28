using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Structs
{
    /// <summary>
    /// Simple configuration used to store message data.
    /// This is created before sending, and allows
    /// the send type to ne defined on creation.
    /// </summary>
    public struct NetOutgoingMessageConfiguration
    {
        public NetDeliveryMethod Method;
        public Int32 SequenceChannel;
        public NetOutgoingMessage Message;
        public NetConnection Recipient;
    }
}
