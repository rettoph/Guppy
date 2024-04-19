using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions;
using Guppy.Engine.Common.Components;
using Guppy.Game.Common;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                scene.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IScene scene in _scenes.GetAll())
            {
                scene.Update(gameTime);
            }
        }
    }
}
