using Guppy.DependencyInjection;
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
                public static readonly Byte UserJoined = 0x0;
                public static readonly Byte UserLeft   = 0x1;

                public static readonly Byte CreateNetworkEntity = 0x2;
                public static readonly Byte UpdateNetworkEntity = 0x3;
                public static readonly Byte PingNetworkEntity   = 0x4;
                public static readonly Byte DeleteNetworkEntity = 0x5;
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
