using Guppy.Common.Implementations;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Common.DependencyInjection;
using System.Runtime.CompilerServices;
using Guppy.Common.DependencyInjection.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterGuppyCommon(this IServiceCollection services)
        {
            return services.AddSingleton<Global>()
                    .AddSingleton(typeof(IGlobal<>), typeof(Global<>))
                    .AddTransient(typeof(Lazy<>), typeof(Lazier<>))
                    .AddTransient(typeof(IScoped<>), typeof(Scoped<>))
                    .AddSingleton<IAliasProvider, AliasProvider>()
                    .AddScoped<IBroker, Broker>()
                    .AddScoped<IBus, Bus>()
                    .AddScoped<BusConfiguration>()
                    .AddScoped<BrokerConfiguration>()
                    .AddScoped<IFilteredProvider, FilteredProvider>()
                    .AddTransient(typeof(IFiltered<>), typeof(Filtered<>));
        }

        private static readonly ConditionalWeakTable<IServiceCollection, IList<IServiceCollectionManager>> _managers = new();
        
        public static TManager GetManager<TManager>(this IServiceCollection services)
            where TManager : IServiceCollectionManager, new()
        {
            if (!_managers.TryGetValue(services, out var managers))
            {
                managers = new List<IServiceCollectionManager>();
                _managers.Add(services, managers);
            }

            var instance = managers.FirstOrDefault(x => x is TManager);

            if(instance is null)
            {
                instance = new TManager();
                managers.Add(instance);
            }

            return (TManager)instance;
        }

        public static IServiceCollection RefreshManagers(this IServiceCollection services)
        {
            if (!_managers.TryGetValue(services, out var managers))
            {
                return services;
            }

            foreach(IServiceCollectionManager manager in managers.OrderBy(x => x.Order))
            {
                manager.Refresh(services);
            }

            return services;
        }

        public static IServiceCollection ConfigureCollection(this IServiceCollection services, Action<IServiceCollectionManager> configure)
        {
            var manager = services.GetManager<ServiceCollectionManager>();
            configure(manager);

            return services;
        }

        public static IServiceConfiguration AddService(this IServiceCollection services, Type serviceType)
        {
            var manager = services.GetManager<ServiceCollectionManager>();

            return manager.AddService(serviceType);
        }

        public static ServiceConfiguration<T> AddService<T>(this IServiceCollection services)
            where T : class
        {
            return (ServiceConfiguration<T>)services.AddService(typeof(T));
        }

        public static IServiceConfiguration GetService(this IServiceCollection services, Type serviceType)
        {
            var manager = services.GetManager<ServiceCollectionManager>();

            return manager.GetService(serviceType);
        }

        public static IServiceConfiguration GetService(this IServiceCollection services, Type serviceType, Func<IServiceConfiguration, bool> predicate)
        {
            var manager = services.GetManager<ServiceCollectionManager>();

            return manager.GetService(serviceType, predicate);
        }

        public static ServiceConfiguration<TService> GetService<TService>(this IServiceCollection services)
            where TService : class
        {
            var manager = services.GetManager<ServiceCollectionManager>();

            return manager.GetService<TService>();
        }

        public static ServiceConfiguration<TService> GetService<TService>(this IServiceCollection services, Func<ServiceConfiguration<TService>, bool> predicate)
            where TService : class
        {
            var manager = services.GetManager<ServiceCollectionManager>();

            return manager.GetService<TService>(predicate);
        }

        public static IServiceCollection RemoveBy(this IServiceCollection services, Func<ServiceDescriptor, bool> predicate, out ServiceDescriptor[] removed)
        {
            removed = services.Where(predicate).ToArray();

            foreach(ServiceDescriptor remove in removed)
            {
                services.Remove(remove);
            }

            return services;
        }
    }
}
