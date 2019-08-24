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
    [IsServiceLoader(200)]
    internal sealed class LoaderServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            AssemblyHelper.GetTypesAssignableFrom<ILoader>().Where(t => t.IsClass && !t.IsAbstract).ForEach(t =>
            { // Register all the internal loader instances...
                services.AddSingleton(t);
                services.AddSingleton<ILoader>(p => p.GetService(t) as ILoader);
            });
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // Automatically load all service loader instances
            provider.GetServices<ILoader>().ForEach(l => l.Load());
        }
    }
}
