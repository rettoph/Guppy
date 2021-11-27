using DotNetUtils.General.Interfaces;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Components;
using Guppy.IO.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.Text;

namespace Guppy.IO.ServiceLoaders
{
    internal sealed class TerminalServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<TerminalService>(p => new TerminalService());
            services.RegisterTypeFactory<IConsole>(p => p.GetService<TerminalService>()).SetPriority(1);
            services.RegisterTypeFactory<GameTerminalComponent>(p => new GameTerminalComponent());

            services.RegisterService<TerminalService>().SetLifetime(ServiceLifetime.Singleton);
            services.RegisterService<GameTerminalComponent>().SetLifetime(ServiceLifetime.Transient);

            services.RegisterComponent<GameTerminalComponent>()
                .SetEntityServiceConfigurationKey<Game>()
                .SetOrder(Guppy.Core.Constants.Orders.ComponentOrder);

            services.RegisterBuilder<Game>((g, p, c) =>
            {
                p.GetService<TerminalService>();
            }, Guppy.Core.Constants.Priorities.PreCreate);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
