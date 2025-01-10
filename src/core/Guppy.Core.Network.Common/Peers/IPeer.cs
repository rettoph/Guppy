using Guppy.Core.Network.Common.Enums;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Common.Peers
{
    public interface IPeer
    {
        PeerTypeEnum Type { get; }
        PeerStateEnum State { get; }
        IUserService Users { get; }
        INetMessageService Messages { get; }
        INetGroupService Groups { get; }
        INetGroup Group { get; }

        void Flush();
    }
}