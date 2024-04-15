using Guppy.Core.Network.Identity;
using Guppy.Core.Network.Identity.Claims;

namespace Guppy.Core.Network.Peers
{
    public interface IClientPeer : IPeer
    {
        User? ServerUser { get; }

        void Start();
        void Connect(string address, int port, params Claim[] claims);
    }
}
