using Guppy.Network.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Peers;
using Guppy.Network.Services;

namespace Guppy.Network
{
    public interface INetScope
    {
        NetScopeState State { get; }

        byte Id { get; }
        IPeer? Peer { get; }

        INetScopeUserService Users { get; }
        INetMessageService Messages { get; }

        void AttachPeer(IPeer peer, byte id);
        void DetachPeer();

        void Enqueue(INetIncomingMessage message);
        void Enqueue(INetOutgoingMessage message);
        void Send(INetOutgoingMessage message);
    }
}
