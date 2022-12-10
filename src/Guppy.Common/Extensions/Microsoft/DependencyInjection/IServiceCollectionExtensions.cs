using Guppy.Common.Implementations;
using Guppy.Common;
using Guppy.Common.Providers;
using Guppy.Common.DependencyInjection;

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
                    .AddScoped<IBus, Bus>()
                    .AddScoped<BusConfiguration>();
        }

        /// <summary>
        /// Add all <see cref="ServiceDescriptor"/> instances returned by
        /// <see cref="IServiceDescriptorProvider.GetDescriptors"/>
        /// </summary>
        /// <typeparam name="TServiceDescriptorProvider"></typeparam>
        /// <param name="services"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IServiceCollection Add<TServiceDescriptorProvider>(this IServiceCollection services, TServiceDescriptorProvider provider)
            where TServiceDescriptorProvider : IServiceDescriptorProvider
        {
            foreach (var descriptor in provider.GetDescriptors())
            {
                services.Add(descriptor);
            }

            return services;
        }

        public static IServiceCollection ConfigureDescriptors(this IServiceCollection services, Action<ServiceDescriptorProvider> configure)
        {
            var provider = new ServiceDescriptorProvider();
            configure(provider);

            return services.Add(provider);
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
