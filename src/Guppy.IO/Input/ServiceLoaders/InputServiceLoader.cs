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
    internal sealed class InputServiceLoader : IServiceLoader
    {
        public void ConfigureServices(ServiceCollection services)
        {
            services.AddFactory<InputService>(p => new InputService());
            services.AddFactory<InputManager>(p => new InputManager());
            services.AddFactory<MouseService>(p => new MouseService());
            services.AddFactory<KeyboardService>(p => new KeyboardService());
            services.AddFactory<InputCommandService>(p => new InputCommandService());
            services.AddFactory<InputCommand>(p => new InputCommand());

            services.AddSingleton<InputService>();
            services.AddTransient<InputManager>();
            services.AddSingleton<MouseService>(autoBuild: true);
            services.AddSingleton<KeyboardService>(autoBuild: true);
            services.AddSingleton<InputCommandService>(autoBuild: true);
            services.AddTransient<InputCommand>();
        }

        public void ConfigureProvider(ServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
