using Guppy.Core.Network.Enums;

namespace Guppy.Core.Network
{
    public interface INetScope
    {
        PeerType Type { get; }
        IReadOnlyList<INetGroup> Groups { get; }

        void Enqueue(INetIncomingMessage message);
        void Enqueue(INetOutgoingMessage message);

        internal void Add(INetGroup group);
        internal void Remove(INetGroup group);
    }
}
