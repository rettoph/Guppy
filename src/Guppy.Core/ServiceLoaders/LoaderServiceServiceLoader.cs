using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using DotNetUtils.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            assemblyHelper.Types.GetTypesWithAutoLoadAttribute(typeof(LoaderService<,,>)).ForEach(loaderServiceType =>
            {
                services.RegisterService(loaderServiceType.FullName)
                    .SetLifetime(ServiceLifetime.Singleton)
                    .SetTypeFactory(loaderServiceType, factory =>
                    {
                        factory.SetMethod(P => Activator.CreateInstance(loaderServiceType));
                    });
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
