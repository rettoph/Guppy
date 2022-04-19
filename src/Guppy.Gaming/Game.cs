﻿using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Guppy.Gaming.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public abstract class Game : Frameable, IAsyncable
    {
        public readonly IEntityService Entities;
        public readonly ISceneService Scenes;

        public Game(ISceneService scenes, IEntityService entities)
        {
            this.Entities = entities;
            this.Scenes = scenes;

            this.Entities.TryAdd(this);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.Scenes.Scene?.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.Scenes.Scene?.Update(gameTime);
        }
    }
}