using Guppy.Attributes;
using Guppy.Extensions;
using Guppy.Extensions.Collection;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader(90)]
    public class LayerServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // throw new NotImplementedException();
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            var loader = provider.GetService<PoolLoader>();

            // Automatically register all Layers types with the IsLayer attribute
            AssemblyHelper.GetTypesWithAttribute<Layer, IsLayerAttribute>().ForEach(layer =>
            {
                loader.TryRegisterLayer(layer);
            });
        }
    }
}
