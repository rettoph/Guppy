using Guppy.ECS.Definitions;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS
{
    public sealed class EntityType
    {
        private Dictionary<Type, IComponentDefinition> _componentDefinitions;

        public readonly EntityKey Key;
        public readonly EntityTag[] Tags;
        public IEnumerable<IComponentDefinition> ComponentDefinitions => _componentDefinitions.Values;

        public EntityType(EntityKey key, EntityTag[] tags, IEnumerable<IComponentDefinition> components)
        {
            this.Key = key;
            this.Tags = tags;

            _componentDefinitions = components.ToDictionary(x => x.Type);
        }

        public Entity Create(World world)
        {
            var entity = world.CreateEntity();

            entity.Attach(this);

            foreach (IComponentDefinition component in _componentDefinitions.Values)
            {
                component.CreateFor(entity);
            }

            return entity;
        }

        public Entity Create(World world, params object[] components)
        {
            var entity = this.Create(world);

            foreach (object component in components)
            {
                _componentDefinitions[component.GetType()].AttachTo(entity, component);
            }

            return entity;
        }
    }
}
