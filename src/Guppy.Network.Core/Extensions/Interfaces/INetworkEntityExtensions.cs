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
        #region No Builder
        public static TNetworkEntityMessage BuildMessage<TNetworkEntityMessage>(this INetworkEntity entity)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = new TNetworkEntityMessage()
            {
                NetworkId = entity.NetworkId,
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
        #endregion

        #region With Builder
        public static TNetworkEntityMessage BuildMessage<TNetworkEntityMessage>(this INetworkEntity entity, Action<TNetworkEntityMessage> builder)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = new TNetworkEntityMessage()
            {
                NetworkId = entity.NetworkId,
            };

            message.Packets.AddRange(entity.Packets.GetAll<TNetworkEntityMessage>());
            builder(message);

            return message;
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, Action<TNetworkEntityMessage> builder)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = entity.BuildMessage<TNetworkEntityMessage>(builder);
            entity.Pipe.SendMessage(message);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, NetPeer recipient, Action<TNetworkEntityMessage> builder)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = entity.BuildMessage<TNetworkEntityMessage>(builder);
            entity.Pipe.SendMessage(message, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this INetworkEntity entity, IEnumerable<NetPeer> recipients, Action<TNetworkEntityMessage> builder)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            TNetworkEntityMessage message = entity.BuildMessage<TNetworkEntityMessage>(builder);
            entity.Pipe.SendMessage(message, recipients);
        }
        #endregion
    }
}
