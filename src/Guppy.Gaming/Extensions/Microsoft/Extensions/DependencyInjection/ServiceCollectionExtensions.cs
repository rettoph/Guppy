using Guppy.EntityComponent;
using Guppy.Gaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScene<TScene>(this IServiceCollection services)
            where TScene : Scene
        {
            return services.AddActivated<Scene, TScene>();
        }

        public static IServiceCollection AddScene<TScene>(this IServiceCollection services, Func<IServiceProvider, TScene> factory)
            where TScene : Scene
        {
            return services.AddActivated<Scene, TScene>(factory);
        }

        public static IServiceCollection AddGame<TGame>(this IServiceCollection services)
            where TGame : Game
        {
            return services.AddActivated<Game, TGame>(singleton: true);
        }

        public static IServiceCollection AddGame<TGame>(this IServiceCollection services, Func<IServiceProvider, TGame> factory)
            where TGame : Game
        {
            return services.AddActivated<Game, TGame>(factory: factory, singleton: true);
        }
    }
}
