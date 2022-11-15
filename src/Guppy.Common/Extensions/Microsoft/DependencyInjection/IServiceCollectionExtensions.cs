using Guppy.Common.Implementations;
using Guppy.Common;
using Guppy.Common.Providers;

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
                    .AddTransient(typeof(IFiltered<>), typeof(Filtered<>))
                    .AddSingleton<IAliasProvider, AliasProvider>()
                    .AddScoped<IBus, Bus>(); ;
        }

        public static IServiceCollection AddMap<T, TImplementation>(this IServiceCollection services, ServiceLifetime? lifetime = null)
            where T : class
            where TImplementation : T

        {
            if(lifetime is null)
            {
                lifetime = services.FirstOrDefault(x => x.ServiceType == typeof(TImplementation))?.Lifetime;
            }

            Func<Func<IServiceProvider, T>, IServiceCollection> addFunc = lifetime switch
            {
                ServiceLifetime.Transient => services.AddTransient<T>,
                ServiceLifetime.Scoped => services.AddTransient<T>,
                ServiceLifetime.Singleton => services.AddSingleton<T>,
                _ => throw new InvalidOperationException()
            };

            return addFunc(p => p.GetRequiredService<TImplementation>());
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
