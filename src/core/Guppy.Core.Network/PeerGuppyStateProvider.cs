using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Network.Common.Enums;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Extensions.Autofac;

namespace Guppy.Core.Network.Common
{
    [AutoLoad]
    internal class PeerGuppyStateProvider : IStateProvider
    {
        private readonly INetScope? _scope;

        public PeerGuppyStateProvider(ILifetimeScope scope)
        {
            if (scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _scope);
            }
        }

        public IEnumerable<IState> GetStates()
        {
            yield return new State<PeerType>(() => _scope?.Type ?? PeerType.None, (x, y) => x.HasFlag(y));
        }
    }
}
