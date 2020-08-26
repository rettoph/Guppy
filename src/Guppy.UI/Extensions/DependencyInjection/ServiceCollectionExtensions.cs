using Guppy.DependencyInjection;
using Guppy.UI.Collections;
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
            where TComponent : IComponent
        {
            // Configure factories...
            services.AddFactory<TComponent>(factory);
            services.AddFactory<ComponentCollection<TComponent>>(p => new ComponentCollection<TComponent>());
            services.AddFactory<Container<TComponent>>(p => new Container<TComponent>());
            services.AddFactory<StackContainer<TComponent>>(p => new StackContainer<TComponent>());

            // Configure service lifetimes...
            services.AddTransient<TComponent>();
            services.AddTransient<ComponentCollection<TComponent>>();
            services.AddTransient<Container<TComponent>>();
            services.AddTransient<StackContainer<TComponent>>();
        }
    }
}
