using Guppy.Attributes;
using Guppy.Extensions.Collection;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    /// <summary>
    /// Service loader used to automatically register all
    /// game types into the service collection.
    /// </summary>
    [IsServiceLoader]
    internal sealed class GameServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GameFactory>();

            AssemblyHelper.GetTypesAssignableFrom<Game>().ForEach(t =>
            {
                // Add each game type as a singleton created via the game factory...
                services.AddSingleton(t, p => p.GetRequiredService<GameFactory>().Build(t));
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
        }
    }
}
