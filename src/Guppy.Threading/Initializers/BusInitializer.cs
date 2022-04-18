using Guppy.Attributes;
using Guppy.Initializers;
using Guppy.Loaders;
using Guppy.Threading.Definitions;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Initializers
{
    internal sealed class BusInitializer : IGuppyInitializer
    {
        public void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders)
        {
            var busMessages = assemblies.GetAttributes<BusMessageDefinition, AutoLoadAttribute>().Types;

            foreach (Type busMessage in busMessages)
            {
                services.AddBusMessage(busMessage);
            }

            services.AddScoped<Bus>();
        }
    }
}
