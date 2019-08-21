using Guppy.Interfaces;
using Guppy.Utilities.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        #region Scene Methods
        public static void AddScene<TScene>(this IServiceCollection services)
            where TScene : Scene
        {
            services.AddGame(typeof(TScene));
        }
        public static void AddScene(this IServiceCollection services, Type sceneType)
        {
            if (!typeof(Scene).IsAssignableFrom(sceneType))
                throw new Exception($"Unable to add scene. Input Type<{sceneType.Name}> does not extend Scene.");

            services.AddScoped(sceneType, p =>
            {
                var configuration = p.GetService<SceneConfiguration>();

                if (configuration.Instance == null)
                    configuration.Instance = ActivatorUtilities.CreateInstance(p, sceneType) as Scene;

                return configuration.Instance;
            });
        }
        #endregion

        #region Game Methods
        public static void AddGame<TGame>(this IServiceCollection services)
            where TGame : Game
        {
            services.AddGame(typeof(TGame));
        }
        public static void AddGame(this IServiceCollection services, Type gameType)
        {
            if (!typeof(Game).IsAssignableFrom(gameType))
                throw new Exception($"Unable to add game. Input Type<{gameType.Name}> does not extend Game.");

            services.AddSingleton(gameType, p =>
            {
                var configuration = p.GetService<GameConfiguration>();

                if (configuration.Instance == null)
                {
                    configuration.Instance = ActivatorUtilities.CreateInstance(p, gameType) as Game;
                    configuration.Instance.TryCreate(p);

                    configuration.Instance.TryPreInitialize();
                    configuration.Instance.TryInitialize();
                    configuration.Instance.TryPostInitialize();
                }

                return configuration.Instance;
            });
        }
        #endregion

        #region Loader Methods
        public static void AddLoader<TLoader>(this IServiceCollection services)
            where TLoader : ILoader
        {
            services.AddLoader(typeof(TLoader));
        }
        public static void AddLoader(this IServiceCollection services, Type loaderType)
        {
            if (!typeof(ILoader).IsAssignableFrom(loaderType))
                throw new Exception($"Unable to add loader. Input Type<{loaderType.Name}> does not extend ILoader.");

            services.AddSingleton(typeof(ILoader), loaderType);
            services.AddSingleton(loaderType, p => p.GetServices<ILoader>().First(l => loaderType == l.GetType()));
        }
        #endregion
    }
}
