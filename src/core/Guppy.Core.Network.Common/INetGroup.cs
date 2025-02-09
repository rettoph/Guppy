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
        IReadOnlyList<IMessageBus> Relays { get; }

        INetOutgoingMessage<T> CreateMessage<T>(in T body)
            where T : notnull;

        void Publish<T>(T im)
            where T : INetIncomingMessage;

        void Add(IMessageBus relay);
        void Remove(IMessageBus relay);
    }
}