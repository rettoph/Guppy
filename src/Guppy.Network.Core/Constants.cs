using Guppy.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Network.Contexts;
using Guppy.Network.Lists;
using Guppy.Network.Services;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public static class Constants
    {
        public static class ServiceConfigurations
        {
            public static readonly ServiceConfigurationKey TransientUserList = ServiceConfigurationKey.From<UserList>("user-list:transient");
        }

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
                public static readonly UInt32 UserJoined = "user:joined".xxHash();
                public static readonly UInt32 UserLeft = "user:left".xxHash();

                public static readonly UInt32 CreateNetworkEntity = "network-scene:network-entity:create".xxHash();
                public static readonly UInt32 UpdateNetworkEntity = "network-scene:network-entity:update".xxHash();
                public static readonly UInt32 PingNetworkEntity = "network-scene:network-entity:ping".xxHash();
                public static readonly UInt32 DeleteNetworkEntity = "network-scene:network-entity:delete".xxHash();
            }

            public static class NetworkEntity
            {
                public static readonly UInt32 Create = "network-entity:create".xxHash();
                public static readonly UInt32 Update = "network-entity:update".xxHash();
                public static readonly UInt32 Ping = "network-entity:ping".xxHash();
                public static readonly UInt32 Delete = "network-entity:delete".xxHash();
            }
        }
    }
}
