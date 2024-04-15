using LiteNetLib;

namespace Guppy.Core.Network.Common
{
    public interface ISender
    {
        NetPeer Peer { get; }
        IUser User { get; }
    }
}
