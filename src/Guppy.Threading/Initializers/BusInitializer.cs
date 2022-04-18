using Guppy.Attributes;
using Guppy.Initializers;
using Guppy.Threading.Loaders;
using Microsoft.Extensions.DependencyInjection;
using Minnow.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Initializers
{
    internal sealed class BusInitializer : GuppyInitializer<IBusLoader>
    {
        protected override void Initialize(IAssemblyProvider assemblies, IServiceCollection services, IEnumerable<IBusLoader> loaders)
        {
            var messages = new BusMessageCollection();

            foreach(IBusLoader loader in loaders)
            {
                loader.ConfigureBus(messages);
            }

            services.AddScoped<Bus>(p => messages.Build());
        }
    }
}
