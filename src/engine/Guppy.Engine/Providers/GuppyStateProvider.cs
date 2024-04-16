using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Autofac;

namespace Guppy.Engine.Providers
{
    [AutoLoad]
    internal sealed class GuppyStateProvider : IStateProvider
    {
        private readonly IGuppy? _guppy;

        public GuppyStateProvider(ILifetimeScope scope)
        {
            if (scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _guppy);
            }
        }

        public IEnumerable<IState> GetStates()
        {
            yield return new State<Type>(StateKey<Type>.Create<IGuppy>(), _guppy?.GetType(), (x, y) => x?.IsAssignableTo(y) ?? false);
        }
    }
}
