using Guppy.Core.Network.Identity.Claims;

namespace Guppy.Core.Network.Peers
{
    public interface IServerPeer : IPeer
    {
        void Start(int port, params Claim[] claims);
    }
}
