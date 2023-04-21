using Guppy;
using Guppy.Common;
using Guppy.Input;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInput<TData>(this IServiceCollection services, string key, ButtonSource defaultSource, (bool, TData)[] data)
            where TData : IMessage
        {
            return services.AddScoped(typeof(IButton), p => new Button<TData>(key, defaultSource, data));
        }
    }
}
