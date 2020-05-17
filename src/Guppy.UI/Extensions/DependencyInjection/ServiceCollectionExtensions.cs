using Guppy.DependencyInjection;
using Guppy.UI.Collections;
using Guppy.UI.Components;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUIComponent<TComponent>(this ServiceCollection services, Func<ServiceProvider, TComponent> factory)
            where TComponent : IComponent
        {
            services.AddTransient<TComponent>(factory);
            services.AddTransient<ComponentCollection<TComponent>>(p => new ComponentCollection<TComponent>());
            services.AddTransient<Container<TComponent>>(p => new Container<TComponent>());
            services.AddTransient<StackContainer<TComponent>>(p => new StackContainer<TComponent>());
        }
    }
}
