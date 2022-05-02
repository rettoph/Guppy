using Guppy.EntityComponent;
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
    public abstract class Game : Frameable, IAsyncable, ISubscribableFrameable
    {
        public readonly IEntityService Entities;
        public readonly ISceneService Scenes;

        public event ISubscribableFrameable.Step? OnDraw;
        public event ISubscribableFrameable.Step? OnUpdate;
        public event ISubscribableFrameable.Step? OnPreDraw;
        public event ISubscribableFrameable.Step? OnPostDraw;
        public event ISubscribableFrameable.Step? OnPreUpdate;
        public event ISubscribableFrameable.Step? OnPostUpdate;

        public Game(
            ISceneService scenes, 
            IEntityService entities)
        {
            this.Entities = entities;
            this.Scenes = scenes;

            this.Entities.TryAdd(this);
        }

        public override void Draw(GameTime gameTime)
        {
            this.OnPreDraw?.Invoke(gameTime);

            base.Draw(gameTime);

            this.OnDraw?.Invoke(gameTime);

            this.Scenes.Scene?.Draw(gameTime);

            this.OnPostDraw?.Invoke(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.OnPreUpdate?.Invoke(gameTime);

            base.Update(gameTime);

            this.OnUpdate?.Invoke(gameTime);

            this.Scenes.Scene?.Update(gameTime);

            this.OnPostUpdate?.Invoke(gameTime);
        }
    }
}
