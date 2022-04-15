using Guppy.EntityComponent.Loaders.Descriptors;
using Guppy.EntityComponent.Providers;

namespace Guppy.EntityComponent.Loaders.Collections
{
    public interface IComponentCollection : IList<ComponentDescriptor>
    {
        IComponentCollection Add<TEntity, TComponent>()
            where TEntity : class, IEntity
            where TComponent : class, IComponent;

        IComponentCollection Add<TEntity, TComponent>(Func<IServiceProvider, TComponent> factory)
            where TEntity : class, IEntity
            where TComponent : class, IComponent;

        IComponentProvider BuildProvider(IEnumerable<Type> entities);
    }
}
