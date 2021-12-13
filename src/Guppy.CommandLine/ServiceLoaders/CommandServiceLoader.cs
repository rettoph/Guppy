using DotNetUtils.DependencyInjection;
using DotNetUtils.General.Interfaces;
using Guppy.Attributes;
using Guppy.CommandLine.Services;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.Text;

namespace Guppy.CommandLine.ServiceLoaders
{
    [AutoLoad]
    internal sealed class CommandServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<IConsole>()
                .SetDefaultConstructor<SystemConsole>()
                .SetPriority(0);

            services.RegisterService<CommandService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<CommandService>();
                });

            services.RegisterService<RootCommand>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetMethod(_ => new RootCommand()
                    {
                        Name = ">‎"
                    });
                });

            services.RegisterService<IConsole>()
                .SetLifetime(ServiceLifetime.Singleton);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
