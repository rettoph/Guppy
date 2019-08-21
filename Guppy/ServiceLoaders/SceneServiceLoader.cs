using Guppy.Attributes;
using Guppy.Extensions.DependencyInjection;
using Guppy.Extensions.Linq;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [IsServiceLoader]
    public class SceneServiceLoader : IServiceLoader
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Automatically register all Scene types with the IsScene attribute
            AssemblyHelper.GetTypesWithAttribute<Scene, IsSceneAttribute>().ForEach(t => services.AddScene(t));
        }

        public void ConfigureProvider(IServiceProvider provider)
        {
            // Automatically register a pool for each scene type
            var poolLoader = provider.GetService<PoolLoader>();
            provider.GetService<GameOptions>().SceneTypes.ForEach(t => poolLoader.TryRegister(t, typeof(ScenePool)));
        }
    }
}
