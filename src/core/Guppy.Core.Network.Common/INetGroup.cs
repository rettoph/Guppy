using Guppy.Core.Messaging.Common;
using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Common
{
    public interface INetGroup
    {
        byte Id { get; }
        IPeer Peer { get; }
        INetScopeUserService Users { get; }
        IReadOnlyList<INetScope> Scopes { get; }
        IReadOnlyList<IBus> Relays { get; }

        INetOutgoingMessage<T> CreateMessage<T>(in T body)
            where T : notnull;

        void Publish(INetIncomingMessage im);

        void Add(IBus relay);
        void Remove(IBus relay);
    }
}
