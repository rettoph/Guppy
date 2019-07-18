using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public struct NetOutgoingMessageConfiguration
    {
        public NetDeliveryMethod Method;
        public Int32 SequenceChannel;
        public NetOutgoingMessage Message;
        public NetConnection Target;
    }
}
