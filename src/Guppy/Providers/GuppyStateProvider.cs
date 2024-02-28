using Autofac;
using Guppy.Attributes;
using Guppy.Common.Autofac;
using Guppy.Extensions.Autofac;
using Guppy.StateMachine;

namespace Guppy.Providers
{
    [AutoLoad]
    internal sealed class GuppyStateProvider : StateProvider
    {
        private readonly IGuppy? _guppy;

        public GuppyStateProvider(ILifetimeScope scope) : base()
        {
            if (scope.HasTag(LifetimeScopeTags.GuppyScope))
            {
                scope.TryResolve(out _guppy);
            }
        }

        public override IEnumerable<IState> GetStates()
        {
            yield return new State<Type>(StateKey<Type>.Create<IGuppy>(), _guppy?.GetType(), (x, y) => x?.IsAssignableTo(y) ?? false);
        }
    }
}
