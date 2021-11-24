using Guppy.Attributes;
using Guppy.DependencyInjection;
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
        public void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<InputButtonService>(p => new InputButtonService());
            services.RegisterTypeFactory<InputButtonManager>(p => new InputButtonManager());
            services.RegisterTypeFactory<MouseService>(p => new MouseService());
            services.RegisterTypeFactory<KeyboardService>(p => new KeyboardService());
            services.RegisterTypeFactory<InputCommandService>(p => new InputCommandService());
            services.RegisterTypeFactory<InputCommand>(p => new InputCommand());

            services.RegisterSingleton<InputButtonService>();
            services.RegisterTransient<InputButtonManager>();
            services.RegisterSingleton<MouseService>();
            services.RegisterSingleton<KeyboardService>();
            services.RegisterSingleton<InputCommandService>();
            services.RegisterTransient<InputCommand>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
