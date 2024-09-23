using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Engine
{
    [AutoLoad]
    [Sequence<InitializeSequence>(InitializeSequence.Initialize)]
    [Sequence<DrawSequence>(DrawSequence.Draw)]
    [Sequence<UpdateSequence>(UpdateSequence.Update)]
    internal class SceneFrameComponent : EngineComponent, IUpdatableComponent, IDrawableComponent, IGuppyDrawable, IGuppyUpdateable
    {
        private readonly ISceneService _scenes;

        public SceneFrameComponent(ISceneService scenes)
        {
            _scenes = scenes;
        }

        [Sequence<DrawComponentSequence>(DrawComponentSequence.Draw)]
        public void Draw(GameTime gameTime)
        {
            foreach (IScene scene in _scenes.GetAll())
            {
                if (scene.Visible == false)
                {
                    continue;
                }

                scene.Draw(gameTime);
            }
        }

        [Sequence<UpdateComponentSequence>(UpdateComponentSequence.Update)]
        public void Update(GameTime gameTime)
        {
            foreach (IScene scene in _scenes.GetAll())
            {
                if (scene.Enabled == false)
                {
                    continue;
                }

                scene.Update(gameTime);
            }
        }
    }
}
