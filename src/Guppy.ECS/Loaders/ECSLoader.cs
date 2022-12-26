using Guppy.ECS.Providers;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Loaders
{
    internal sealed class ECSLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IWorldProvider, WorldProvider>();
            //services.AddScoped<World>(p => p.GetRequiredService<IWorldProvider>().Get(null));
        }
    }
}
