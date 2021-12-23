using Guppy.Network.Messages;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Interfaces
{
    internal static class INetworkEntityExtensions
    {
        public static TNetworkEntityMessage BuildMessage<TNetworkEntityMessage>(this INetworkEntity entity)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = new TNetworkEntityMessage()
            {
                NetworkId = entity.NetworkId,
                ServiceConfigurationId = entity.ServiceConfiguration.Id,
            };

            message.Packets.AddRange(entity.Packets.GetAll<TNetworkEntityMessage>());

            return message;
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = entity.BuildMessage<TNetworkEntityMessage>();
            entity.Pipe.SendMessage(message);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = entity.BuildMessage<TNetworkEntityMessage>();
            entity.Pipe.SendMessage(message, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = entity.BuildMessage<TNetworkEntityMessage>();
            entity.Pipe.SendMessage(message, recipients);
        }
    }
}
