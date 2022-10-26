using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    internal sealed class EntityDefinition : IEntityDefinition
    {
        public EntityKey Key { get; }
        public EntityTag[] Tags { get; }

        public EntityDefinition(EntityKey key, EntityTag[] tags)
        {
            this.Key = key;
            this.Tags = tags;
        }
    }
}
