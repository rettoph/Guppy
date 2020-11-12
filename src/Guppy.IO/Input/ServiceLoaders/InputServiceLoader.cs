using Guppy.Attributes;
using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Interfaces;
using Guppy.IO.Input.Services;
using Guppy.IO.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Input.ServiceLoaders
{
    [AutoLoad(-1)]
    internal sealed class InputServiceLoader : IServiceLoader
    {
        public void RegisterServices(ServiceCollection services)
        {
            services.AddFactory<InputService>(p => new InputService());
            services.AddFactory<InputManager>(p => new InputManager());
            services.AddFactory<MouseService>(p => new MouseService());
            services.AddFactory<KeyboardService>(p => new KeyboardService());
            services.AddFactory<InputCommandService>(p => new InputCommandService());
            services.AddFactory<InputCommand>(p => new InputCommand());

            services.AddSingleton<InputService>();
            services.AddTransient<InputManager>();
            services.AddSingleton<MouseService>();
            services.AddSingleton<KeyboardService>();
            services.AddSingleton<InputCommandService>();
            services.AddTransient<InputCommand>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // Auto-build required services...
            provider.GetService<MouseService>();
            provider.GetService<KeyboardService>();
            provider.GetService<InputCommandService>();
        }
    }
}
