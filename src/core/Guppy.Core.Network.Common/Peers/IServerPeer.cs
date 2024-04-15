using Guppy.Core.Network.Common.Claims;

namespace Guppy.Core.Network.Common.Peers
{
    public interface IServerPeer : IPeer
    {
        void Start(int port, params Claim[] claims);
    }
}
