using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Network.Common;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;

namespace Guppy.Core.Network
{
    [AutoLoad]
    internal class PeerGuppyStateProvider : BaseStateProvider
    {
        private readonly IEnumerable<INetScope> _scopes;

        public PeerGuppyStateProvider(ILifetimeScope scope)
        {
            IEnumerable<INetScope>? scopes = null;

            if (scope.IsRoot() == false)
            {

                scope.TryResolve(out scopes);
            }

            _scopes = scopes ?? Enumerable.Empty<INetScope>();
        }

        public override bool TryGet(IStateKey key, out object? state)
        {
            switch (key)
            {
                case IStateKey<PeerType> { Value: StateKey.DefaultValue }:
                    state = _scopes.Select(x => x.Group.Peer.Type).Aggregate((x, y) => x | y);
                    return true;
                default:
                    state = null;
                    return false;
            }
        }
    }
}
