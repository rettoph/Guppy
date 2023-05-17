using MonoGame.Extended.Entities;

namespace Guppy.ECS.Services
{
    public interface IEntityService
    {
        void Make(EntityType type, Entity entity);

        Entity Create(EntityType type);
        Entity Create(EntityType type, Action<Entity> factory);

        void Destroy(int entityId);
        void Destroy(int entityId, out EntityBackup backup);

        Entity Restore(EntityBackup backup);

        IEntityService Configure(EntityType type, Action<EntityConfiguration> configurator);
    }
}
