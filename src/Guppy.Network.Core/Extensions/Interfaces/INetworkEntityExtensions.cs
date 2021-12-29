using Guppy.Network.Interfaces;
using Guppy.Network.Messages;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public static class INetworkEntityExtensions
    {
        /// <summary>
        /// Populate the recieved <typeparamref name="TNetworkEntityMessage"/>'s
        /// <see cref="NetworkEntityMessage.NetworkId"/> and 
        /// <see cref="NetworkEntityMessage.Packets"/>.
        /// </summary>
        /// <typeparam name="TNetworkEntityMessage"></typeparam>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        public static void PopulateMessage<TNetworkEntityMessage>(this INetworkEntity entity, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            message.NetworkId = entity.NetworkId;
            message.Packets.AddRange(entity.Messages.GetAll<TNetworkEntityMessage>());
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            entity.Pipe.SendMessage(message);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, TNetworkEntityMessage message, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            entity.Pipe.SendMessage(message, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, TNetworkEntityMessage message, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            entity.Pipe.SendMessage(message, recipients);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage());
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), recipients);
        }
    }
}
