using Autofac;
using Guppy.Engine.Attributes;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Extensions.Autofac;
using Guppy.Core.StateMachine;

namespace Guppy.Engine.Providers
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
