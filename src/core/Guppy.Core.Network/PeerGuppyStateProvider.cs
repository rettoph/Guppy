using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Extensions.Autofac;
using Guppy.Core.Network.Enums;
using Guppy.Engine.Providers;
using Guppy.Core.StateMachine;

namespace Guppy.Core.Network
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
