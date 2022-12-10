using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Common.DependencyInjection
{
    public static class ServiceCollectionManagerExtensions
    {
        public static ServiceConfiguration<T> AddService<T>(this ServiceCollectionManager manager)
            where T : class
        {
            return (ServiceConfiguration<T>)manager.AddService(typeof(T));
        }

        public static ServiceConfiguration<T> AddSingleton<T>(this ServiceCollectionManager manager)
            where T : class
        {
            return manager.AddService<T>().SetLifetime(ServiceLifetime.Singleton);
        }

        public static ServiceConfiguration<T> AddScoped<T>(this ServiceCollectionManager manager)
            where T : class
        {
            return manager.AddService<T>().SetLifetime(ServiceLifetime.Scoped);
        }

        public static ServiceConfiguration<T> AddTransient<T>(this ServiceCollectionManager manager)
            where T : class
        {
            return manager.AddService<T>().SetLifetime(ServiceLifetime.Singleton);
        }

        public static ServiceConfiguration<T> AddSingleton<T, TImplementation>(this ServiceCollectionManager manager)
            where T : class
            where TImplementation : class, T
        {
            return manager.AddSingleton<T>().SetImplementationType<TImplementation>();
        }

        public static ServiceConfiguration<T> AddScoped<T, TImplementation>(this ServiceCollectionManager manager)
            where T : class
            where TImplementation : class, T
        {
            return manager.AddScoped<T>().SetImplementationType<TImplementation>();
        }

        public static ServiceConfiguration<T> AddTransient<T, TImplementation>(this ServiceCollectionManager manager)
            where T : class
            where TImplementation : class, T
        {
            return manager.AddTransient<T>().SetImplementationType<TImplementation>();
        }

        public static ServiceConfiguration<T> GetService<T>(this ServiceCollectionManager manager)
            where T : class
        {
            return (ServiceConfiguration<T>)manager.GetService(typeof(T));
        }

        public static ServiceConfiguration<T> GetService<T>(this ServiceCollectionManager manager, Func<ServiceConfiguration<T>, bool> predicate)
            where T : class
        {
            return (ServiceConfiguration<T>)manager.GetService(typeof(T), c => predicate((ServiceConfiguration<T>)c));
        }
    }
}
