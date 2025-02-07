using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Guppy.Game.Common.Systems;
using Microsoft.Xna.Framework;

namespace Guppy.Game.Systems.Engine
{
    public class SceneFrameSystem(ISceneService scenes) : IEngineSystem, IUpdateSystem, IDrawSystem
    {
        private readonly ISceneService _scenes = scenes;

        [SequenceGroup<DrawSequenceGroupEnum>(DrawSequenceGroupEnum.Draw)]
        public void Draw(GameTime gameTime)
        {
            foreach (IScene scene in this._scenes.GetAll())
            {
                if (scene.Visible == false)
                {
                    continue;
                }

                scene.Draw(gameTime);
            }
        }

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.Update)]
        public void Update(GameTime gameTime)
        {
            foreach (IScene scene in this._scenes.GetAll())
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