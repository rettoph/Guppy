using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Game.Common;

namespace Guppy.Engine.Providers
{
    [AutoLoad]
    internal sealed class SceneStateProvider : BaseStateProvider
    {
        private readonly IScene? _scene;

        public SceneStateProvider(ILifetimeScope scope)
        {
            if (scope.IsRoot() == false)
            {
                scope.TryResolve(out _scene);
            }
        }

        public override bool TryGet(IStateKey key, out object? state)
        {
            switch (key)
            {
                case IStateKey<Type> { Value: nameof(IScene) }:
                    state = _scene?.GetType();
                    return true;
                default:
                    state = null;
                    return false;
            }
        }
    }
}
