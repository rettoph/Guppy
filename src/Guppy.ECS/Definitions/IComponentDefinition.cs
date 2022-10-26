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

        void AttachTo(Entity entity, object instance);

        void CreateFor(Entity entity);
    }
}
