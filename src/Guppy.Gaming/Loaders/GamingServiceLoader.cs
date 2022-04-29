using Guppy.Gaming.Components;
using Guppy.Gaming.Providers;
using Guppy.Gaming.Services;
using Guppy.Loaders;
using Guppy.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Guppy.Gaming.Loaders
{
    internal sealed class GamingServiceLoader<TGame> : IServiceLoader
        where TGame : Game
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGame<TGame>();

            services.AddSingleton<ISceneService, SceneService>();
            services.AddScoped<Bus>();
            services.AddScoped<ITimerProvider, TimerProvider>();

            services.AddComponent<Game, UpdateTimersComponent<Game>>();
            services.AddComponent<Scene, UpdateTimersComponent<Scene>>();
        }
    }
}
