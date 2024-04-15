using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.StateMachine;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Autofac;
using Guppy.Engine.Extensions.Autofac;

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
