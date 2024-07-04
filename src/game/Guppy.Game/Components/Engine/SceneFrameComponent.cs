using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Game.Common;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Engine
{
    [AutoLoad]
    internal class SceneFrameComponent : EngineComponent, IGuppyUpdateable, IGuppyDrawable
    {
        private readonly ISceneService _scenes;

        public SceneFrameComponent(ISceneService scenes)
        {
            _scenes = scenes;
        }

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
