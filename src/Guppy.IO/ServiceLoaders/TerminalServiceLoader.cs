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
        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<Terminal>(p => new Terminal());
            services.RegisterTypeFactory<IConsole>(p => p.GetService<Terminal>());
            services.RegisterTypeFactory<GameTerminalComponent>(p => new GameTerminalComponent());

            services.RegisterSingleton<Terminal>();
            services.RegisterTransient<GameTerminalComponent>();

            services.RegisterComponent<GameTerminalComponent, Game>();

            services.RegisterBuilder<Game>((g, p, c) =>
            {
                p.GetService<Terminal>();
            }, Guppy.Core.Constants.Priorities.PreCreate);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
