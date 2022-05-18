using Guppy;
using Guppy.Attributes;
using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using Guppy.EntityComponent.Loaders;
using Guppy.EntityComponent.Providers;
using Guppy.Initializers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent.Initializers
{
    internal sealed class SetupInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var setups = assemblies.GetAttributes<SetupDefinition, AutoLoadAttribute>().Types;

            foreach (Type setup in setups)
            {
                services.AddSetup(setup);
            }

            services.AddScoped<ISetupProvider, SetupProvider>();
        }
    }
}
