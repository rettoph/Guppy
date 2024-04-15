using Guppy.Core.Network.Common.Enums;

namespace Guppy.Core.Network.Common
{
    public interface INetScope
    {
        PeerType Type { get; }
        IReadOnlyList<INetGroup> Groups { get; }

        void Enqueue(INetIncomingMessage message);
        void Enqueue(INetOutgoingMessage message);
    }
}
