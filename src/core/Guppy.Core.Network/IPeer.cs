using Guppy.Core.Network.Enums;
using Guppy.Core.Network.Identity.Services;
using Guppy.Core.Network.Services;

namespace Guppy.Core.Network
{
    public interface IPeer
    {
        PeerType Type { get; }
        Enums.PeerState State { get; }
        IUserService Users { get; }
        INetMessageService Messages { get; }
        INetGroupService Groups { get; }
        INetScope DefaultNetScope { get; }

        void Flush();

        internal void Send(INetOutgoingMessage message);
    }
}
