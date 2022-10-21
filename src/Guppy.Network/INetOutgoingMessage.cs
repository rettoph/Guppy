using Guppy.Common;
using LiteNetLib;
using LiteNetLib.Utils;
using static System.Formats.Asn1.AsnWriter;

namespace Guppy.Network
{
    public interface INetOutgoingMessage : IMessage, INetMessage, IRecyclable
    {
        object Body { get; }
        IEnumerable<object> Data { get; }
        NetDataWriter Writer { get; }
        NetMessageType Type { get; }

        INetOutgoingMessage Append<TData>(in TData value)
            where TData : notnull;

        INetOutgoingMessage AddRecipient(NetPeer recipient);

        INetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients);

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

        new INetOutgoingMessage<T> Send();

        new INetOutgoingMessage<T> Enqueue();
    }
}