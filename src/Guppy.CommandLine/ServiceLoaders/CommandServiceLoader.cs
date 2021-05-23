using Guppy.Attributes;
using Guppy.CommandLine.Services;
using Guppy.DependencyInjection;
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
        public void RegisterServices(ServiceCollection services)
        {
            services.RegisterTypeFactory<Commands>(p => new Services.Commands());
            services.RegisterTypeFactory<RootCommand>(p => new RootCommand()
            {
                Name = ">‎"
            });
            services.RegisterTypeFactory<IConsole>(p => new SystemConsole());

            services.RegisterSingleton<Commands>();
            services.RegisterSingleton<RootCommand>();
            services.RegisterSingleton<IConsole>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
