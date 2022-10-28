using Guppy.Common;
using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    public interface INetIncomingMessage : IMessage, INetMessage, IRecyclable
    {
        NetPeer? Peer { get; }
        object Body { get; }
        IEnumerable<object> Data { get; }
        NetMessageType Type { get; }

        public void Read(NetPeer? peer, NetDataReader reader);
        INetIncomingMessage Enqueue();
    }

    public interface INetIncomingMessage<T> : INetIncomingMessage
        where T : notnull
    {
        new T Body { get; }

        new NetMessageType<T> Type { get; }

        new INetIncomingMessage<T> Enqueue();
    }
}
