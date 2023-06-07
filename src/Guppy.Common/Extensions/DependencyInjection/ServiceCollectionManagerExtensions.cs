using Guppy.Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Common.DependencyInjection
{
    public static class ServiceCollectionManagerExtensions
    {
        public static ServiceConfiguration<T> AddService<T>(this IServiceCollectionManager manager)
            where T : class
        {
            return (ServiceConfiguration<T>)manager.AddService(typeof(T));
        }

        public static ServiceConfiguration<T> AddSingleton<T>(this IServiceCollectionManager manager)
            where T : class
        {
            return manager.AddService<T>().SetLifetime(ServiceLifetime.Singleton);
        }

        public static ServiceConfiguration<T> AddScoped<T>(this IServiceCollectionManager manager)
            where T : class
        {
            return manager.AddService<T>().SetLifetime(ServiceLifetime.Scoped);
        }

        public static ServiceConfiguration<T> AddTransient<T>(this IServiceCollectionManager manager)
            where T : class
        {
            return manager.AddService<T>().SetLifetime(ServiceLifetime.Transient);
        }

        public static ServiceConfiguration<T> GetService<T>(this IServiceCollectionManager manager)
            where T : class
        {
            return (ServiceConfiguration<T>)manager.GetService(typeof(T));
        }

        public static ServiceConfiguration<T> GetService<T>(this IServiceCollectionManager manager, Func<ServiceConfiguration<T>, bool> predicate)
            where T : class
        {
            return (ServiceConfiguration<T>)manager.GetService(typeof(T), c => predicate((ServiceConfiguration<T>)c));
        }
    }
}
