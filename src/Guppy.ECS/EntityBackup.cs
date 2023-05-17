
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS
{
    public class EntityBackup
    {
        public readonly EntityType Type;
        public readonly (Type type, object instance)[] Components;

        public EntityBackup(EntityType type, (Type type, object instance)[] components)
        {
            this.Type = type;
            this.Components = components;
        }
    }
}
