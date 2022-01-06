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
using Guppy.ServiceLoaders;
using Serilog;
using Serilog.Core;

namespace Guppy.GuppyInitializers
{
    [AutoLoad]
    internal sealed class SerilogLoaderGuppyInitializer : IGuppyInitializer
    {
        public void PreInitialize(
            AssemblyHelper assemblies, 
            ServiceProviderBuilder services, 
            IEnumerable<IGuppyLoader> loaders)
        {
            LoggerConfiguration logConfiguration = new LoggerConfiguration();

            foreach (IGuppyLoader loader in loaders)
            {
                if(loader is ISerilogLoader serilogLoader)
                {
                    serilogLoader.RegisterSerilog(logConfiguration);
                }
            }

            services.RegisterService<ILogger>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => logConfiguration.CreateLogger());
                });
        }

        public void PostInitialize(
            ServiceProvider provider, 
            IEnumerable<IGuppyLoader> loaders)
        {
            // throw new NotImplementedException();
        }
    }
}
