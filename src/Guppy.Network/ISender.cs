using Guppy.Network.Identity;
using LiteNetLib;

namespace Guppy.Network
{
    public interface ISender
    {
        NetPeer Peer { get; internal set; }
        User User { get; }
    }
}
