using Guppy.EntityComponent.DependencyInjection;
using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Lists;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class SceneServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
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
    }
}
