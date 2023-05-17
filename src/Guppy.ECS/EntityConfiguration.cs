using Guppy.ECS.Services;
using MonoGame.Extended.Entities;

namespace Guppy.ECS
{
    public class EntityConfiguration
    {
        public readonly EntityType Type;

        private Action<Entity>? _factories;
        private readonly HashSet<EntityType> _baseTypes;
        private readonly IEntityService _entities;

        public EntityConfiguration(EntityType type, IEntityService entities)
        {
            _entities = entities;
            _baseTypes = new HashSet<EntityType>();

            this.Type = type;
        }

        public EntityConfiguration Inherit(EntityType baseType)
        {
            _baseTypes.Add(baseType);

            return this;
        }

        public EntityConfiguration AttachComponent<T>(Func<Entity, T> factory)
            where T : class
        {
            _factories += e =>
            {
                if(e.Has<T>())
                {
                    return;
                }

                e.Attach(factory(e));
            };

            return this;
        }

        public EntityConfiguration EnsureComponent<T>()
            where T : class
        {
            _factories += e =>
            {
                if (e.Has<T>())
                {
                    return;
                }

                throw new InvalidOperationException($"Error creating EntityType  '{this.Type.Name}', Missing required component '{typeof(T).FullName}'.");
            };

            return this;
        }

        public void Make(Entity entity)
        {
            _factories?.Invoke(entity);

            foreach (EntityType baseType in _baseTypes)
            {
                _entities.Make(baseType, entity);
            }
        }
    }
}
