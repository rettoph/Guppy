using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS
{
    public class EntityType : IEquatable<EntityType?>
    {
        public readonly string Name;

        public EntityType(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as EntityType);
        }

        public bool Equals(EntityType? other)
        {
            return other is not null &&
                   Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(EntityType? left, EntityType? right)
        {
            return EqualityComparer<EntityType>.Default.Equals(left, right);
        }

        public static bool operator !=(EntityType? left, EntityType? right)
        {
            return !(left == right);
        }
    }
}
