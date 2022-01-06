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
        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, Pipe pipe)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            pipe.SendMessage(message);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.SendMessage(message, entity.Pipe);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, Pipe pipe, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            pipe.SendMessage(message, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.SendMessage(message, entity.Pipe, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, Pipe pipe, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.PopulateMessage(message);
            pipe.SendMessage(message, recipients);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, TNetworkEntityMessage message, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>
        {
            entity.SendMessage(message, entity.Pipe, recipients);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, Pipe pipe)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), pipe);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), entity.Pipe);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, Pipe pipe, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), pipe, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, NetPeer recipient)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), entity.Pipe, recipient);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, Pipe pipe, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), pipe, recipients);
        }

        public static void SendMessage<TNetworkEntityMessage>(this IMagicNetworkEntity entity, IEnumerable<NetPeer> recipients)
            where TNetworkEntityMessage : NetworkEntityMessage<TNetworkEntityMessage>, new()
        {
            entity.SendMessage(new TNetworkEntityMessage(), entity.Pipe, recipients);
        }
    }
}
