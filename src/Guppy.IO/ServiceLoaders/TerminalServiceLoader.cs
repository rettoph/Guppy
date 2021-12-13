using DotNetUtils.DependencyInjection;
using DotNetUtils.General.Interfaces;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
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
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<IConsole>()
                .SetMethod(p => p.GetService<TerminalService>())
                .SetPriority(1);

            services.RegisterService<TerminalService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<TerminalService>();
                });

            services.RegisterService<GameTerminalComponent>()
                .SetLifetime(ServiceLifetime.Transient).SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<GameTerminalComponent>();
                });


            services.RegisterComponent<GameTerminalComponent>()
                .SetAssignableEntityType<Game>()
                .SetOrder(Guppy.Core.Constants.Orders.ComponentOrder);

            services.RegisterBuilder<Game>()
                .SetOrder(Guppy.Core.Constants.Priorities.PreCreate)
                .SetMethod((g, p, c) =>
                {
                    p.GetService<TerminalService>();
                });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
