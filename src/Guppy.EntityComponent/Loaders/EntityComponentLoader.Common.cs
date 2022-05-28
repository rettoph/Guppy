using Guppy.Attributes;
using Guppy.EntityComponent.Definitions.Setups;
using Guppy.EntityComponent.Providers;
using Guppy.EntityComponent.Services;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Loaders
{
    [AutoLoad]
    internal sealed class EntityComponentCommonLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSetup<ComponentSetupDefinition>();
            services.AddSetup<InitializableSetupDefinition>();
        }
    }
}
