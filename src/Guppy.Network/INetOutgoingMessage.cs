using Guppy.Messaging;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    public interface INetOutgoingMessage : IMessage, INetMessage, IRecyclable
    {
        object Body { get; }
        byte OutgoingChannel { get; }
        DeliveryMethod DeliveryMethod { get; }
        NetDataWriter Writer { get; }
        new NetMessageType Type { get; }

        INetOutgoingMessage AddRecipient(NetPeer recipient);

        INetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients);

        INetOutgoingMessage SetOutgoingChannel(byte outgoingChannel);

        INetOutgoingMessage SetDeliveryMethod(DeliveryMethod deliveryMethod);

        INetOutgoingMessage Send();

        INetOutgoingMessage Enqueue();
    }

    public interface INetOutgoingMessage<T> : INetOutgoingMessage
        where T : notnull
    {
        new T Body { get; }

        new NetMessageType<T> Type { get; }

        void Write(in T body);

        new INetOutgoingMessage<T> AddRecipient(NetPeer recipient);

        new INetOutgoingMessage<T> AddRecipients(IEnumerable<NetPeer> recipients);

        new INetOutgoingMessage<T> SetOutgoingChannel(byte outgoingChannel);

        new INetOutgoingMessage<T> SetDeliveryMethod(DeliveryMethod deliveryMethod);

        new INetOutgoingMessage<T> Send();

        new INetOutgoingMessage<T> Enqueue();
    }
}