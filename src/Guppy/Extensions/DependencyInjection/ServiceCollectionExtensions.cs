using Guppy.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        #region Scene Methods
        public static void AddScene(this ServiceCollection services, Type scene, Func<ServiceProvider, Object> factory)
        {
            services.ServiceDescriptors.Add(new Guppy.DependencyInjection.ServiceDescriptor()
            {
                ServiceType = scene,
                CacheType = typeof(Scene),
                Lifetime = Guppy.DependencyInjection.ServiceLifetime.Scoped,
                Factory = (p) => factory(p),
                Priority = 0
            });
        }
        public static void AddScene<TScene>(this ServiceCollection services, Func<ServiceProvider, TScene> factory)
        {
            services.AddScene(typeof(TScene), (p) => factory(p));
        }
        #endregion
    }
}
