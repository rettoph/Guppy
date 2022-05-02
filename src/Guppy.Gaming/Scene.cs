using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Gaming
{
    public abstract class Scene : Frameable, ISubscribableFrameable
    {
        public readonly IEntityService Entities;

        public event ISubscribableFrameable.Step? OnDraw;
        public event ISubscribableFrameable.Step? OnUpdate;
        public event ISubscribableFrameable.Step? OnPreDraw;
        public event ISubscribableFrameable.Step? OnPostDraw;
        public event ISubscribableFrameable.Step? OnPreUpdate;
        public event ISubscribableFrameable.Step? OnPostUpdate;

        public Scene(IEntityService entities)
        {
            this.Entities = entities;

            this.Entities.TryAdd(this);
        }

        public override void Draw(GameTime gameTime)
        {
            this.OnPreDraw?.Invoke(gameTime);

            base.Draw(gameTime);

            this.OnDraw?.Invoke(gameTime);

            this.OnPostDraw?.Invoke(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            this.OnPreUpdate?.Invoke(gameTime);

            base.Update(gameTime);

            this.OnUpdate?.Invoke(gameTime);

            this.OnPostUpdate?.Invoke(gameTime);
        }
    }
}