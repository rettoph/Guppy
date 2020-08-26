using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using Guppy.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            AssemblyHelper.GetTypesWithAutoLoadAttribute(typeof(LoaderService<,,>)).ForEach(s =>
            {
                services.AddFactory(s, p => ActivatorUtilities.CreateInstance(p, s));
                services.AddSingleton(s, s);
            });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
