using Guppy.Network.Identity;
using Guppy.Network.Identity.Claims;

namespace Guppy.Network.Peers
{
    public interface IClientPeer : IPeer
    {
        User? ServerUser { get; }

        void Start();
        void Connect(string address, int port, params Claim[] claims);
    }
}
