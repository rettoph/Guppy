using Guppy.EntityComponent.DependencyInjection;
using Guppy.Attributes;
using Guppy.Interfaces;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.EntityComponent.DependencyInjection.Builders;

namespace Guppy.ServiceLoaders
{
    internal sealed class ContentServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterService<ContentService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .RegisterTypeFactory(factory =>
                {
                    factory.SetMethod(p => new ContentService());
                });

            services.RegisterSetup<ContentService>()
                .SetMethod((content, p, c) =>
                {
                    content.TryRegister(Constants.Content.DebugFont, "Fonts/DiagnosticsFont");
                });
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
