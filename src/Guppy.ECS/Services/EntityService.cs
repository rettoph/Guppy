using Guppy.Common.Collections;
using Guppy.ECS.Definitions;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Services
{
    internal sealed class EntityService : IEntityService
    {
        private World _world;
        private DoubleDictionary<EntityKey, uint, EntityType> _types;
        
        public EntityService(
            World world, 
            IEnumerable<IEntityTypeDefinition> types, 
            IEnumerable<IComponentDefinition> components)
        {
            _world = world;

            _types = types.GroupBy(x => x.Key)
                .Select(x => BuildEntityType(x.Key, x.SelectMany(t => t.Tags), components))
                .ToDoubleDictionary(x => x.Key, x => x.Key.Id);
        }

        private static EntityType BuildEntityType(EntityKey key, IEnumerable<EntityTag> tags, IEnumerable<IComponentDefinition> components)
        {
            components = components.Where(x => x.Tags.Any(t => tags.Contains(t)));

            return new EntityType(key, tags.ToArray(), components);
        }

        public Entity Create(EntityKey key)
        {
            return _types[key].Create(_world);
        }

        public Entity Create(EntityKey key, params object[] components)
        {
            return _types[key].Create(_world, components);
        }

        public Entity Create(uint keyId)
        {
            return _types[keyId].Create(_world);
        }

        public Entity Create(uint keyId, params object[] components)
        {
            return _types[keyId].Create(_world, components);
        }
    }
}
