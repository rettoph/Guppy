using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LayerableServiceCollection : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<LayerableList>(p => new LayerableList());
            services.RegisterService<LayerableList>().SetLifetime(ServiceLifetime.Scoped);

            services.RegisterSetup<ILayerable>((l, p, c) =>
            {
                p.GetService<LayerableList>().TryAdd(l);
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
