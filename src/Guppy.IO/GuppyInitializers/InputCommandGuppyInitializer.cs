using Guppy.Attributes;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using Guppy.IO.Builders;
using Guppy.IO.Services;
using Guppy.ServiceLoaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.IO.GuppyInitializers
{
    [AutoLoad]
    internal sealed class InputCommandGuppyInitializer : IGuppyInitializer
    {
        public void PreInitialize(AssemblyHelper assemblies, ServiceProviderBuilder services, IEnumerable<IGuppyLoader> loaders)
        {
            InputCommandServiceBuilder builder = new InputCommandServiceBuilder();

            foreach (IGuppyLoader loader in loaders)
            {
                if (loader is IInputCommandLoader inputCommandLoader)
                {
                    inputCommandLoader.RegisterInputCommands(builder);
                }
            }

            services.RegisterService<InputCommandService>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => builder.Build(p));
                });

            services.RegisterService<InputCommand>()
                .SetLifetime(ServiceLifetime.Transient)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<InputCommand>();
                });
        }

        public void PostInitialize(ServiceProvider provider, IEnumerable<IGuppyLoader> loaders)
        {
            // throw new NotImplementedException();
        }
    }
}
