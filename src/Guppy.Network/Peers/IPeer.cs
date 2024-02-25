using Guppy.Network.Enums;
using Guppy.Network.Identity.Services;

namespace Guppy.Network.Peers
{
    public interface IPeer
    {
        PeerType Type { get; }
        IUserService Users { get; }
        IReadOnlyDictionary<byte, INetScope> Scopes { get; }

        internal bool TryAttachNetScope(INetScope scope, byte id);
        internal void DetachNetScope(INetScope scope);

        void Flush();

        internal void Send(INetOutgoingMessage message);
    }
}
