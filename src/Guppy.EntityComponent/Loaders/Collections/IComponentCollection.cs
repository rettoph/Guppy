using Guppy.EntityComponent.Loaders.Definitions;
using Guppy.EntityComponent.Loaders.Descriptors;
using Guppy.EntityComponent.Providers;

namespace Guppy.EntityComponent.Loaders.Collections
{
    public interface IComponentCollection : IList<ComponentDescriptor>
    {
        IComponentCollection Add<TEntity, TComponent>()
            where TEntity : class, IEntity
            where TComponent : class, IComponent;

        IComponentCollection Add<TEntity, TComponent>(Func<IServiceProvider, TEntity, TComponent> factory)
            where TEntity : class, IEntity
            where TComponent : class, IComponent;

        IComponentCollection Add<TDefinition>()
            where TDefinition : ComponentDefinition;
    }
}
