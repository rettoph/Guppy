using Guppy.DependencyInjection;
using Guppy.UI.Lists;
using Guppy.UI.Components;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.UI.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUIComponent<TComponent>(this ServiceCollection services, Func<ServiceProvider, TComponent> factory)
            where TComponent : class, IComponent
        {
            // Configure factories...
            services.AddFactory<TComponent>(factory);
            services.AddFactory<ComponentList<TComponent>>(p => new ComponentList<TComponent>());
            services.AddFactory<Container<TComponent>>(p => new Container<TComponent>());
            services.AddFactory<StackContainer<TComponent>>(p => new StackContainer<TComponent>());

            // Configure service lifetimes...
            services.AddTransient<TComponent>();
            services.AddTransient<ComponentList<TComponent>>();
            services.AddTransient<Container<TComponent>>();
            services.AddTransient<StackContainer<TComponent>>();
        }
    }
}
