using Guppy.EntityComponent.Loaders.Descriptors;
using Guppy.EntityComponent.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders.Collections
{
    public interface ISetupCollection : IList<SetupDescriptor>
    {
        ISetupCollection Add<TEntity, TSetup>(
            Func<IServiceProvider, TSetup> factory,
            int order)
                where TEntity : class, IEntity
                where TSetup : class, ISetup;

        ISetupCollection Add<TEntity, TSetup>(
            int order)
                where TEntity : class, IEntity
                where TSetup : class, ISetup;
    }
}
