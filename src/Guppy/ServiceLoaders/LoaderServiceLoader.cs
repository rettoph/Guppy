using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Interfaces;
using Guppy.Loaders;
using Guppy.Utilities;
using Guppy.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.ServiceLoaders
{
    [AutoLoad]
    internal sealed class LoaderServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            AssemblyHelper.GetTypesWithAutoLoadAttribute(typeof(Loader<,,>)).ForEach(s =>
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
