using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Game.Common;

namespace Guppy.Engine.Providers
{
    [AutoLoad]
    internal sealed class SceneStateProvider : IStateProvider
    {
        private readonly IScene? _scene;

        public SceneStateProvider(ILifetimeScope scope)
        {
            if (scope.IsRoot() == false)
            {
                scope.TryResolve(out _scene);
            }
        }

        public IEnumerable<IState> GetStates()
        {
            yield return new State<Type>(StateKey<Type>.Create<IScene>(), _scene?.GetType(), (x, y) => x?.IsAssignableTo(y) ?? false);
        }
    }
}
