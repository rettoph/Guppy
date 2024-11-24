using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Game;
using Guppy.Game.Common;

namespace Guppy.Engine.Providers
{
    internal sealed class SceneStateProvider : BaseStateProvider
    {
        private readonly IScene? _scene;
        private readonly ISceneConfiguration? _configuration;

        public SceneStateProvider(ILifetimeScope scope)
        {
            if (scope.IsRoot() == false)
            {
                scope.TryResolve(out _scene);
                scope.TryResolve(out _configuration);
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
                    return this.TryGetConfiguration(key, out state);
            }
        }

        private bool TryGetConfiguration(IStateKey key, out object? configuration)
        {
            if (_configuration is null)
            {
                configuration = null;
                return false;
            }

            return _configuration.TryGet(key.Value, out configuration);
        }
    }
}
