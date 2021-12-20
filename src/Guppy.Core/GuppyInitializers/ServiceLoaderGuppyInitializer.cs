using Guppy.EntityComponent.DependencyInjection;
using Minnow.General;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.Interfaces;
using Guppy.EntityComponent.Attributes;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using Guppy.Attributes;

namespace Guppy.GuppyInitializers
{
    [AutoLoad]
    internal sealed class ServiceLoaderGuppyInitializer : IGuppyInitializer
    {
        public void PreInitialize(
            AssemblyHelper assemblies, 
            ServiceProviderBuilder services, 
            IEnumerable<IGuppyLoader> loaders)
        {
            foreach(IGuppyLoader loader in loaders)
            {
                if(loader is IServiceLoader serviceLoader)
                {
                    serviceLoader.RegisterServices(assemblies, services);
                }
            }
        }

        public void PostInitialize(
            ServiceProvider provider, 
            IEnumerable<IGuppyLoader> loaders)
        {
            // throw new NotImplementedException();
        }
    }
}
