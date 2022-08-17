using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    public interface IComponentDefinition
    {
        Type Type { get; }
        EntityTag[] Tags { get; }

        public abstract void AttachTo(Entity entity, object instance);

        public abstract void CreateFor(Entity entity);
    }
}
