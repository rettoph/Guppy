using Guppy.Common.Implementations;
using Guppy.Common;
using Guppy.Common.Providers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterGuppyCommon(this IServiceCollection services)
        {
            return services.AddSingleton(typeof(IGlobal<>), typeof(Global<>))
                    .AddTransient(typeof(Lazy<>), typeof(Lazier<>))
                    .AddTransient(typeof(IScoped<>), typeof(Scoped<>))
                    .AddTransient(typeof(IFiltered<>), typeof(Filtered<>))
                    .AddSingleton<IAliasProvider, AliasProvider>()
                    .AddScoped<IBus, Bus>(); ;
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
