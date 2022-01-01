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
    public static class IMagicNetworkEntityExtensions
    {
        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            entity.Pipe.SendMessage(message);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            entity.Pipe.SendMessage(message, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            entity.Pipe.SendMessage(message, recipients);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage());
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), recipients);
        }
    }
}
