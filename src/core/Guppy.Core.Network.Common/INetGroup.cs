using Guppy.Core.Network.Common.Peers;
using Guppy.Core.Network.Common.Services;

namespace Guppy.Core.Network.Common
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
    }
}
