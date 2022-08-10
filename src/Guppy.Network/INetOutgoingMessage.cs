using LiteNetLib;
using LiteNetLib.Utils;

namespace Guppy.Network
{
    public interface INetOutgoingMessage : INetMessage
    {
        NetDataWriter Writer { get; }

        INetOutgoingMessage Append<TValue>(in TValue value);

        INetOutgoingMessage AddRecipient(NetPeer recipient);

        INetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients);

        INetOutgoingMessage Send();
    }
}