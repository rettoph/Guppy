using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;

namespace Guppy.Core.Network.Common
{
    [AutoLoad]
    internal class PeerGuppyStateProvider : IStateProvider
    {
        private readonly IEnumerable<INetScope> _scopes;

        public PeerGuppyStateProvider(ILifetimeScope scope)
        {
            if (scope.IsRoot() == false)
            {
                scope.TryResolve(out _scopes);
            }

            _scopes ??= Enumerable.Empty<INetScope>();
        }

        public IEnumerable<IState> GetStates()
        {
            PeerType flags = _scopes.Select(x => x.Group.Peer.Type).Aggregate((x, y) => x | y);
            yield return new State<PeerType>(() => flags, (x, y) => x.HasFlag(y));
        }
    }
}
