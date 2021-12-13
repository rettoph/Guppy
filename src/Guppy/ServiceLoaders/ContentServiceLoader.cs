using DotNetUtils.DependencyInjection;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.ServiceLoaders
{
    internal sealed class ContentServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<ContentService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetMethod(p => new ContentService());
                });

            services.RegisterSetup<ContentService>()
                .SetMethod((content, p, c) =>
                {
                    content.TryRegister(Constants.Content.DebugFont, "Fonts/DiagnosticsFont");
                });
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
