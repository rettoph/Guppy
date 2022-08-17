using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Definitions
{
    internal sealed class EntityTypeDefinition : IEntityTypeDefinition
    {
        public EntityKey Key { get; }
        public EntityTag[] Tags { get; }

        public EntityTypeDefinition(EntityKey key, EntityTag[] tags)
        {
            this.Key = key;
            this.Tags = tags;
        }
    }
}
