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
        public void RegisterServices(GuppyServiceCollection services)
        {
            services.RegisterTypeFactory<InputButtons>(p => new InputButtons());
            services.RegisterTypeFactory<InputButtonManager>(p => new InputButtonManager());
            services.RegisterTypeFactory<Mouse>(p => new Mouse());
            services.RegisterTypeFactory<Keyboard>(p => new Keyboard());

            services.RegisterSingleton<InputButtons>();
            services.RegisterTransient<InputButtonManager>();
            services.RegisterSingleton<Mouse>();
            services.RegisterSingleton<Keyboard>();
        }

        public void ConfigureProvider(GuppyServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
