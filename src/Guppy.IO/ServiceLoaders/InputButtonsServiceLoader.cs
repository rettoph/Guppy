using DotNetUtils.DependencyInjection;
using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.ServiceLoaders
{
    [AutoLoad(-1)]
    internal sealed class InputButtonsServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services)
        {
            services.RegisterService<InputButtonService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<InputButtonService>();
                });

            services.RegisterService<InputButtonManager>()
                .SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<InputButtonManager>();
                });

            services.RegisterService<MouseService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<MouseService>();
                });

            services.RegisterService<KeyboardService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<KeyboardService>();
                });

            services.RegisterService<InputCommandService>()
                .SetLifetime(ServiceLifetime.Singleton)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<InputCommandService>();
                });

            services.RegisterService<InputCommand>()
                .SetLifetime(ServiceLifetime.Transient)
                .SetTypeFactory(factory =>
                {
                    factory.SetDefaultConstructor<InputCommand>();
                });

        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
