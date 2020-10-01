using Guppy.DependencyInjection;
using Guppy.Extensions.DependencyInjection;
using Guppy.Lists;
using Guppy.UI.Elements;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUIComponent<TComponent>(this ServiceCollection services, Func<ServiceProvider, TComponent> factory)
            where TComponent : class, IElement
        {
            // Configure factories...
            services.AddFactory<TComponent>(factory);
            services.AddFactory<ServiceList<TComponent>>(p => new ServiceList<TComponent>());
            services.AddFactory<Container<TComponent>>(p => new Container<TComponent>());

            // Configure service lifetimes...
            services.AddTransient<TComponent>();
            services.AddTransient<ServiceList<TComponent>>();
            services.AddTransient<Container<TComponent>>();
        }
    }
}
