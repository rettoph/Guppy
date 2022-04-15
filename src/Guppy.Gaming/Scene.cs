using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Gaming
{
    public abstract class Scene : Frameable
    {
        public readonly IEntityService Entities;

        public Scene(IEntityService entities)
        {
            this.Entities = entities;

            this.Entities.TryAdd(this);
        }
    }
}