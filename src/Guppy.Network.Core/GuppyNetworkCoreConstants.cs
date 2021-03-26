using Guppy.Extensions.System;
using Guppy.Network.Contexts;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public static class GuppyNetworkCoreConstants
    {
        public static class MessageContexts
        {
            public readonly static NetOutgoingMessageContext InternalReliableDefault = new NetOutgoingMessageContext()
            {
                Method = NetDeliveryMethod.ReliableOrdered,
                SequenceChannel = 15
            };

            public readonly static NetOutgoingMessageContext InternalReliableSecondary = new NetOutgoingMessageContext()
            {
                Method = NetDeliveryMethod.ReliableOrdered,
                SequenceChannel = 20
            };

            public readonly static NetOutgoingMessageContext InternalUnreliableDefault = new NetOutgoingMessageContext()
            {
                Method = NetDeliveryMethod.Unreliable,
                SequenceChannel = 0
            };
        }

        public static class Messages
        {
            public static class Channel
            {
                public static UInt32 UserJoined = "user:joined".xxHash();
                public static UInt32 UserLeft = "user:left".xxHash();
            }
        }

        public static class ServiceConfigurations
        {
            public const String TransientUserList = "user-list:transient";
        }
    }
}
