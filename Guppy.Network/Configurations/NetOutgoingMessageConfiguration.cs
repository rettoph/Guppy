using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    /// <summary>
    /// Simple configuration used to store message data.
    /// These objects should be pooled.
    /// </summary>
    public class NetOutgoingMessageConfiguration
    {
        public NetDeliveryMethod Method;
        public Int32 SequenceChannel;
        public NetOutgoingMessage Message;
        public NetConnection Recipient;
    }
}
