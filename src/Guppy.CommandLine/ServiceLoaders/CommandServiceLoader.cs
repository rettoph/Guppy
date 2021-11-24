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
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<CommandService>(p => new Services.CommandService());
            services.RegisterTypeFactory<RootCommand>(p => new RootCommand()
            {
                Name = ">‎"
            });
            services.RegisterTypeFactory<IConsole>(p => new SystemConsole());

            services.RegisterSingleton<CommandService>();
            services.RegisterSingleton<RootCommand>();
            services.RegisterSingleton<IConsole>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
