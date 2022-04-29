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

        public Scene(IEntityService entities)
        {
            this.Entities = entities;

            this.Entities.TryAdd(this);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.OnDraw?.Invoke(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            this.OnUpdate?.Invoke(gameTime);
        }
    }
}