using Guppy.Gaming.Components;
using Guppy.Gaming.Definitions.Setups;
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
            services.AddScoped<ILayerService, LayerService>();
            services.AddScoped<ILayerableService, LayerableService>();
            services.AddSingleton<Bus>();
            services.AddScoped<ITimerProvider, TimerProvider>();

            services.AddComponent<Game, UpdateTimersComponent<Game>>();
            services.AddComponent<Game, GameCommandsComponent>();
            services.AddComponent<Scene, UpdateTimersComponent<Scene>>();

            services.AddSetup<LayerableSetupDefinition>();
            services.AddSetup<LayerSetupDefinition>();
        }
    }
}
