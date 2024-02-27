using Guppy.Network.Enums;
using Guppy.Network.Identity.Services;
using Guppy.Network.Services;

namespace Guppy.Network
{
    public interface IPeer
    {
        PeerType Type { get; }
        IUserService Users { get; }
        INetMessageService Messages { get; }
        INetGroupService Groups { get; }
        INetScope DefaultNetScope { get; }

        void Flush();

        internal void Send(INetOutgoingMessage message);
    }
}
