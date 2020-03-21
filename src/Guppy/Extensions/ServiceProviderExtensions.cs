using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this ServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }

        public static T GetService<T>(this ServiceProvider provider, Action<ServiceProvider, T> setup)
            where T : IService
        {
            return (T)provider.GetService(typeof(T), (p, i) => setup?.Invoke(p, (T)i));
        }

        public static T GetService<T>(this ServiceProvider provider, UInt32 id, Action<ServiceProvider, T> setup)
            where T : IService
        {
            return (T)provider.GetService(id, (p, i) => setup?.Invoke(p, (T)i));
        }

        public static T GetService<T>(this ServiceProvider provider, String handle, Action<ServiceProvider, T> setup)
            where T : IService
        {
            return (T)provider.GetService(handle, (p, i) => setup?.Invoke(p, (T)i));
        }
    }
}
