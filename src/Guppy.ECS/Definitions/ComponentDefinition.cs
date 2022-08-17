using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    public abstract class ComponentDefinition<T> : IComponentDefinition
        where T : class
    {
        public Type Type => typeof(T);

        public abstract EntityTag[] Tags { get; }

        public ComponentDefinition()
        {
        }

        public virtual void AttachTo(Entity entity, object instance)
        {
            if(instance is T component)
            {
                entity.Attach(component);
            }
        }

        public virtual void CreateFor(Entity entity)
        {
            entity.Attach(this.Factory());
        }

        public abstract T Factory();
    }
}
