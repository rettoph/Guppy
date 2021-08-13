using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.System.Collections;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Extensions.System;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceServiceLoader : IServiceLoader
    {
        public void RegisterServices(DependencyInjection.GuppyServiceCollection services)
        {
            AssemblyHelper.Types.GetTypesWithAutoLoadAttribute(typeof(LoaderService<,,>)).ForEach(s =>
            {
                services.RegisterTypeFactory(s, p => Activator.CreateInstance(s));
                services.RegisterSingleton(ServiceConfigurationKey.From(s));
            });
        }

        public void ConfigureProvider(DependencyInjection.GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
