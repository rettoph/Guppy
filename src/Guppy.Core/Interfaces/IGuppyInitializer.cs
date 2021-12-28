using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.ServiceLoaders;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Interfaces
{
    public interface IGuppyInitializer
    {
        void PreInitialize(
            AssemblyHelper assemblies,
            ServiceProviderBuilder services,
            IEnumerable<IGuppyLoader> loaders);

        void PostInitialize(
            ServiceProvider provider,
            IEnumerable<IGuppyLoader> loaders);
    }
}
