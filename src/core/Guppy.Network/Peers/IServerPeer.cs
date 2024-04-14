using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Peers
{
    public interface IServerPeer : IPeer
    {
        void Start(int port, params Claim[] claims);
    }
}
