using Guppy.Core.Network.Identity.Services;
using Guppy.Core.Network.Messages;

namespace Guppy.Core.Network
{
    public interface INetGroup
    {
        byte Id { get; }
        IPeer Peer { get; }
        INetScopeUserService Users { get; }
        INetScope Scope { get; }

        INetOutgoingMessage<T> CreateMessage<T>(in T body)
            where T : notnull;

        void Attach(INetScope scope);
        void Detach();

        internal void Process(INetIncomingMessage<UserAction> message);
    }
}
