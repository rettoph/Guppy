using Guppy.Demo.Entities;
using Guppy.Demo.Layers;
using Guppy.Demo.Scenes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Utilities.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Demo.ServiceLoaders
{
    public class DemoServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            services.AddSingleton<Random>(new Random(1337));
        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            var contentLoader = provider.GetLoader<ContentLoader>();
            contentLoader.TryRegister("texture:test", "test");

            var entityLoader = provider.GetLoader<EntityLoader>();
            entityLoader.TryRegister<DemoEntity>("entity:demo");
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
