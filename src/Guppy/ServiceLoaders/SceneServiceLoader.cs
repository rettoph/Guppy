using DotNetUtils.DependencyInjection;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class SceneServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<SceneList>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<SceneList>();
                });

            assemblyHelper.Types.GetTypesWithAutoLoadAttribute<IScene>(false).ForEach(s =>
            {
                services.RegisterScene(s);
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
