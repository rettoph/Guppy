using Guppy.EntityComponent;
using Guppy.EntityComponent.Services;

namespace Guppy.EntityComponent
{
    public abstract class Scene : Entity
    {
        public readonly IEntityService Entities;

        public Scene(IEntityService entities)
        {
            this.Entities = entities;

            this.Entities.TryAdd(this);
        }
    }
}