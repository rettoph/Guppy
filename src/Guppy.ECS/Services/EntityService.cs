using Guppy.Common;
using Guppy.ECS.Loaders;
using MonoGame.Extended.Entities;

namespace Guppy.ECS.Services
{
    internal sealed class EntityService : IEntityService
    {
        private readonly World _world;
        private readonly Dictionary<EntityType, EntityConfiguration> _configurations;
        private ComponentMapper<EntityType> _entityTypes;

        public EntityService(World world, ISorted<IEntityLoader> loaders)
        {
            _world = world;
            _entityTypes = world.ComponentManager.GetMapper<EntityType>();
            _configurations = new Dictionary<EntityType, EntityConfiguration>();

            foreach(IEntityLoader loader in loaders)
            {
                loader.Configure(this);
            }
        }

        public Entity Create(EntityType type)
        {
            Entity entity = _world.CreateEntity();

            this.Make(type, entity);

            return entity;
        }

        public Entity Create(EntityType type, Action<Entity> factory)
        {
            Entity entity = _world.CreateEntity();
            factory(entity);

            this.Make(type, entity);

            return entity;
        }

        public void Destroy(int entityId)
        {
            _world.DestroyEntity(entityId);
        }

        public void Destroy(int entityId, out EntityBackup backup)
        {
            backup = new EntityBackup(
                type: _entityTypes.Get(entityId),
                components: _world.ComponentManager.GetAll<object>(entityId)
                    .ToArray()
                );

            this.Destroy(entityId);
        }

        public Entity Restore(EntityBackup backup)
        {
            Entity entity = _world.CreateEntity();
            
            foreach((Type type, object instance) component in backup.Components)
            {
                _world.ComponentManager.Put(entity.Id, component.type, component.instance);
            }

            this.Make(backup.Type, entity);

            return entity;
        }

        public void Make(EntityType type, Entity entity)
        {
            _configurations[type].Make(entity);
            entity.Attach(type);
        }

        public IEntityService Configure(EntityType type, Action<EntityConfiguration> configurator)
        {
            if(!_configurations.TryGetValue(type, out EntityConfiguration? configuration))
            {
                configuration = new EntityConfiguration(type, this);
                _configurations[type] = configuration;
            }

            configurator(configuration);

            return this;
        }
    }
}
