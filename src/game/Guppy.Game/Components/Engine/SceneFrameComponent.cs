using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Common;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Components.Engine
{
    internal class SceneFrameComponent(ISceneService scenes) : IEngineComponent, IUpdatableComponent, IDrawableComponent
    {
        private readonly ISceneService _scenes = scenes;

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.Draw)]
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

        [SequenceGroup<UpdateComponentSequenceGroup>(UpdateComponentSequenceGroup.Update)]
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
