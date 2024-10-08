using Guppy.Core.Common.Attributes;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Providers;
using Guppy.Game.Graphics.Common.Constants;
using Guppy.Game.Graphics.Common.Enums;
using Guppy.Game.MonoGame.Common.Extensions;

namespace Guppy.Game.Graphics.Common
{
    [AutoLoad]
    internal class GraphicsEnabledStateProvider(IGraphicsDevice graphics, ISceneConfiguration? sceneConfiguration = null) : BaseStateProvider
    {
        private readonly IGraphicsDevice _graphics = graphics;
        private readonly ISceneConfiguration? _sceneConfiguration = sceneConfiguration;

        public override bool TryGet(IStateKey key, out object? state)
        {
            switch (key)
            {
                case IStateKey<bool> { Value: GraphicsStateKeys.GraphicsEnabled }:
                    bool result = _graphics.Status == GraphicsObjectStatusEnum.Implemented;
                    if (_sceneConfiguration is not null)
                    {
                        result &= _sceneConfiguration.GetSceneHasGraphicsEnabled();
                    }

                    state = result;
                    return true;
                default:
                    state = null;
                    return false;
            }
        }
    }
}
