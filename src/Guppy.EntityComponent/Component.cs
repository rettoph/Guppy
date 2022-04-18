using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public abstract class Component : IComponent
    {
        public virtual void Dispose()
        {

        }
    }

    public abstract class Component<TEntity> : Component
        where TEntity : IEntity
    {
        public readonly TEntity Entity;

        public Component(TEntity entity)
        {
            this.Entity = entity;
        }
    }
}
