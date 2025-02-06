using Guppy.Core.Common;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Extensions;
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

        public SceneStateProvider(IGuppyScope scope)
        {
            if (scope.GetScopeType() == GuppyScopeTypeEnum.Child)
            {
                scope.TryResolve(out this._scene);
                scope.TryResolve(out this._configuration);
            }
        }

        public override bool TryGet(IStateKey key, out object? state)
        {
            switch (key)
            {
                case IStateKey<Type> { Value: nameof(IScene) }:
                    state = this._scene?.GetType();
                    return true;
                default:
                    return this.TryGetConfiguration(key, out state);
            }
        }

        private bool TryGetConfiguration(IStateKey key, out object? configuration)
        {
            if (this._configuration is null)
            {
                configuration = null;
                return false;
            }

            return this._configuration.TryGet(key.Value, out configuration);
        }
    }
}