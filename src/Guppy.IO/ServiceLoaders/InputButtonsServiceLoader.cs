using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.ServiceLoaders
{
    [AutoLoad(-1)]
    internal sealed class InputButtonsServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<InputButtonService>(p => new InputButtonService());
            services.RegisterTypeFactory<InputButtonManager>(p => new InputButtonManager());
            services.RegisterTypeFactory<MouseService>(p => new MouseService());
            services.RegisterTypeFactory<KeyboardService>(p => new KeyboardService());
            services.RegisterTypeFactory<InputCommandService>(p => new InputCommandService());
            services.RegisterTypeFactory<InputCommand>(p => new InputCommand());

            services.RegisterService<InputButtonService>().SetLifetime(ServiceLifetime.Singleton);
            services.RegisterService<InputButtonManager>().SetLifetime(ServiceLifetime.Transient);
            services.RegisterService<MouseService>().SetLifetime(ServiceLifetime.Singleton);
            services.RegisterService<KeyboardService>().SetLifetime(ServiceLifetime.Singleton);
            services.RegisterService<InputCommandService>().SetLifetime(ServiceLifetime.Singleton);
            services.RegisterService<InputCommand>().SetLifetime(ServiceLifetime.Transient);
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
