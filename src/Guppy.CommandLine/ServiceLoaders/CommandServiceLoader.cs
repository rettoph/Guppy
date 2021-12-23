using Guppy.EntityComponent.DependencyInjection;
using Minnow.General.Interfaces;
using Guppy.Attributes;
using Guppy.CommandLine.Services;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.IO;
using System.Text;
using Guppy.EntityComponent.DependencyInjection.Builders;
using System.CommandLine.Binding;

namespace Guppy.CommandLine.ServiceLoaders
{
    [AutoLoad]
    internal sealed class CommandServiceLoader : IServiceLoader
    {
        class Test
        {
            public String Name { get; set; }
            public Int32 Age;
        }
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterTypeFactory<IConsole>()
                .SetDefaultConstructor<SystemConsole>()
                .SetPriority(0);

            services.RegisterService<IConsole>()
                .SetLifetime(ServiceLifetime.Singleton);
        }
    }
}
