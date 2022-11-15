using Guppy.Common;
using LiteNetLib;
using LiteNetLib.Utils;
using static System.Formats.Asn1.AsnWriter;

namespace Guppy.Network
{
    public interface INetOutgoingMessage : IMessage, INetMessage, IRecyclable
    {
        object Body { get; }
        byte OutgoingChannel { get; }
        DeliveryMethod DeliveryMethod { get; }
        IEnumerable<object> Data { get; }
        NetDataWriter Writer { get; }
        NetMessageType Type { get; }

        INetOutgoingMessage Append<TData>(in TData value)
            where TData : notnull;

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

        new INetOutgoingMessage<T> Append<TData>(in TData value)
            where TData : notnull;

        new INetOutgoingMessage<T> AddRecipient(NetPeer recipient);

        new INetOutgoingMessage<T> AddRecipients(IEnumerable<NetPeer> recipients);

        new INetOutgoingMessage<T> SetOutgoingChannel(byte outgoingChannel);

        new INetOutgoingMessage<T> SetDeliveryMethod(DeliveryMethod deliveryMethod);

        new INetOutgoingMessage<T> Send();

        new INetOutgoingMessage<T> Enqueue();
    }
}