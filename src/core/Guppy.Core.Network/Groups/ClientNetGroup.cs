using Guppy.Core.Network.Common.Peers;

namespace Guppy.Core.Network.Groups
{
    internal sealed class ClientNetGroup(byte id, IPeer peer) : BaseNetGroup(id, peer)
    {
    }
}