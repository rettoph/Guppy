﻿using Guppy.Core.Messaging.Common;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common
{
    public interface INetOutgoingMessage : IMessage, INetMessage, IRecyclable
    {
        object Body { get; }
        byte OutgoingChannel { get; }
        DeliveryMethod DeliveryMethod { get; }
        NetDataWriter Writer { get; }
        new INetMessageType Type { get; }
        IReadOnlyList<NetPeer> Recipients { get; }

        INetOutgoingMessage AddRecipient(NetPeer recipient);

        INetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients);

        INetOutgoingMessage SetOutgoingChannel(byte outgoingChannel);

        INetOutgoingMessage SetDeliveryMethod(DeliveryMethod deliveryMethod);

        INetOutgoingMessage Send();
    }

    public interface INetOutgoingMessage<T> : INetOutgoingMessage
        where T : notnull
    {
        new T Body { get; }

        new INetMessageType<T> Type { get; }

        void Write(in INetGroup group, in T body);

        new INetOutgoingMessage<T> AddRecipient(NetPeer recipient);

        new INetOutgoingMessage<T> AddRecipients(IEnumerable<NetPeer> recipients);

        new INetOutgoingMessage<T> SetOutgoingChannel(byte outgoingChannel);

        new INetOutgoingMessage<T> SetDeliveryMethod(DeliveryMethod deliveryMethod);

        new INetOutgoingMessage<T> Send();
    }
}