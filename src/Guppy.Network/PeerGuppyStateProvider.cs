using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;
using Guppy.Network.Enums;
using Guppy.Providers;
using Guppy.StateMachine;

namespace Guppy.Network
{
    [AutoLoad]
    internal class PeerGuppyStateProvider : StateProvider
    {
        private readonly INetScope? _scope;

        public PeerGuppyStateProvider(ILifetimeScope scope)
        {
            if (scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _scope);
            }
        }

        public override IEnumerable<IState> GetStates()
        {
            yield return new State<PeerType>(() => _scope?.Type ?? PeerType.None, (x, y) => x.HasFlag(y));
        }
    }
}
