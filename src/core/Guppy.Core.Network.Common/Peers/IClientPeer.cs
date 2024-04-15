using Guppy.Core.Network.Common.Claims;

namespace Guppy.Core.Network.Common.Peers
{
    public interface IClientPeer : IPeer
    {
        IUser? ServerUser { get; }

        void Start();
        void Connect(string address, int port, params Claim[] claims);
    }
}
