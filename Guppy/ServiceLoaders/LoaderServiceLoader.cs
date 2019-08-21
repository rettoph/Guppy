using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Extensions.Linq;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    public class LoaderServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Automatically register all Game types with the IsGame attribute
            AssemblyHelper.GetTypesWithAttribute<ILoader, IsLoaderAttribute>().ForEach(t => services.AddLoader(t));
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // Ensure that all loaders are loaded at this time
            provider.GetServices<ILoader>().ForEach(l => l.Load());
        }
    }
}
