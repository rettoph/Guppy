using Guppy.EntityComponent.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
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
