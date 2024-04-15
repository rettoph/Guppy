using Guppy.Core.Network.Identity;
using LiteNetLib;

namespace Guppy.Core.Network
{
    public interface ISender
    {
        NetPeer Peer { get; internal set; }
        User User { get; }
    }
}
