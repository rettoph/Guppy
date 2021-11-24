using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Components;
using Guppy.IO.Services;
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
            services.RegisterTypeFactory<IConsole>(p => p.GetService<TerminalService>(), 0, 1);
            services.RegisterTypeFactory<GameTerminalComponent>(p => new GameTerminalComponent());

            services.RegisterSingleton<TerminalService>();
            services.RegisterTransient<GameTerminalComponent>();

            services.RegisterComponent<GameTerminalComponent, Game>(Guppy.Core.Constants.Orders.ComponentOrder);

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
