using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Peers
{
    public interface IClientPeer : IPeer
    {
        void Start();
        void Connect(string address, int port, params Claim[] claims);
    }
}
