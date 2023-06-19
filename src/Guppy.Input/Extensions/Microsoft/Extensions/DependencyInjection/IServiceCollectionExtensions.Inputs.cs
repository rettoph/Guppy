using Guppy;
using Guppy.Common;
using Guppy.Input;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddInput<TData>(this IServiceCollection services, string key, ButtonSource defaultSource, (bool, TData)[] data)
            where TData : IMessage
        {
            return services.AddScoped(typeof(IButton), p => new Button<TData>(key, defaultSource, data));
        }

        public static IServiceCollection AddInput<TData>(this IServiceCollection services, string key, ButtonSource defaultSource, (ButtonState, TData)[] data)
            where TData : IMessage
        {
            return services.AddScoped(typeof(IButton), p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == ButtonState.Pressed, x.Item2)).ToArray()));
        }
        
        public static IServiceCollection AddInput<TData>(this IServiceCollection services, string key, ButtonSource defaultSource, (KeyState, TData)[] data)
            where TData : IMessage
        {
            return services.AddScoped(typeof(IButton), p => new Button<TData>(key, defaultSource, data.Select(x => (x.Item1 == KeyState.Down, x.Item2)).ToArray()));
        }

    }
}
