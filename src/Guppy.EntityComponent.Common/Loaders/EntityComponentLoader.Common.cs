using Guppy.Attributes;
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
    internal sealed class EntityComponentCommonLoader : IServiceLoader, ISetupLoader
    {
        public void ConfigureSetups(ISetupCollection setups)
        {
            setups.Add<IEntity, ComponentSetup>(Constants.SetupOrders.ComponentSetupOrder);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISetupService>(p => p.GetRequiredService<ISetupProvider>().Create(p));
        }
    }
}
