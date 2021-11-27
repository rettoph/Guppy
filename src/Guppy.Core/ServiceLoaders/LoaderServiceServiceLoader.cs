using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            assemblyHelper.Types.GetTypesWithAutoLoadAttribute(typeof(LoaderService<,,>)).ForEach(s =>
            {
                services.RegisterTypeFactory(s, p => Activator.CreateInstance(s));
                services.RegisterService(ServiceConfigurationKey.From(s))
                    .SetLifetime(ServiceLifetime.Singleton);
            });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
