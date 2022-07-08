using Guppy.Common.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Initializers
{
    public abstract class GuppyInitializer<TLoader> : IGuppyInitializer
        where TLoader : IGuppyLoader
    {
        void IGuppyInitializer.Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var castedLoaders = loaders.Where(l => l is TLoader).Select(l => (TLoader)l);

            this.Initialize(assemblies, services, castedLoaders);
        }

        protected abstract void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<TLoader> loaders);
    }
}
