using Guppy.EntityComponent.DependencyInjection;
using Minnow.General.Interfaces;
using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.IO.Components;
using Guppy.IO.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.Text;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.ServiceLoaders;

namespace Guppy.IO.ServiceLoaders
{
    internal sealed class TerminalServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<IConsole>()
                .SetMethod(p => p.GetService<TerminalService>())
                .SetPriority(1);

            services.RegisterService<TerminalService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<TerminalService>();
                });

            services.RegisterService<GameTerminalComponent>()
                .SetLifetime(ServiceLifetime.Transient).RegisterTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<GameTerminalComponent>();
                });


            services.RegisterComponent<GameTerminalComponent>()
                .SetAssignableEntityType<Game>()
                .SetOrder(EntityComponent.Constants.Orders.ComponentOrder);

            services.RegisterBuilder<Game>()
                .SetOrder(EntityComponent.Constants.Priorities.PreCreate)
                .SetMethod((g, p, c) =>
                {
                    p.GetService<TerminalService>();
                });
        }
    }
}
