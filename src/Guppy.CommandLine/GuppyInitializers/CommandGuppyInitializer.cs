﻿using Guppy.Attributes;
using Guppy.CommandLine.Builders;
using Guppy.CommandLine.Interfaces;
using Guppy.CommandLine.Services;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.GuppyInitializers
{
    [AutoLoad]
    internal sealed class CommandGuppyInitializer : IGuppyInitializer
    {
        public void PreInitialize(AssemblyHelper assemblies, ServiceProviderBuilder services, IEnumerable<IGuppyLoader> loaders)
        {
            CommandServiceBuilder builder = new CommandServiceBuilder();

            foreach (Type commandDefinitionType in assemblies.Types.GetTypesWithAutoLoadAttribute<CommandDefinition>())
            {
                builder.ImportCommandDefinition(commandDefinitionType);
            }

            foreach (IGuppyLoader loader in loaders)
            {
                if(loader is ICommandLoader commandLoader)
                {
                    commandLoader.RegisterCommands(builder);
                }
            }

            services.RegisterService<CommandService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => builder.Build());
                });
        }

        public void PostInitialize(ServiceProvider provider, IEnumerable<IGuppyLoader> loaders)
        {
            // throw new NotImplementedException();
        }
    }
}