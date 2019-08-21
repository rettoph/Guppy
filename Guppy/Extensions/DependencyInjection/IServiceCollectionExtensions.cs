using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Factories;
using Guppy.Utilities.Options;
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
        public static void AddScene<TScene>(this IServiceCollection services, Boolean creatable = true)
            where TScene : Scene
        {
            services.AddScene(typeof(TScene), creatable);
        }
        public static void AddScene(this IServiceCollection services, Type sceneType, Boolean creatable = true)
        {
            ExceptionHelper.ValidateAssignableFrom<Scene>(sceneType);

            if (creatable)
            { // Only add the scene type to the game options if this particular type can be created.
                services.Configure<GameOptions>(go =>
                { // Add the scene type to the game options list, so that the scenes types can be added to the pooling service
                    go.SceneTypes.Add(sceneType);
                });
            }

            services.AddTransient(sceneType, p => p.GetService<SceneOptions>().Instance);
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
            ExceptionHelper.ValidateAssignableFrom<Game>(gameType);

            services.AddSingleton(gameType, p =>
            {
                var options = p.GetService<GameOptions>();

                if (options.Instance == null)
                {
                    options.Instance = ActivatorUtilities.CreateInstance(p, gameType) as Game;
                    options.Instance.TryCreate(p);

                    options.Instance.TryPreInitialize();
                    options.Instance.TryInitialize();
                    options.Instance.TryPostInitialize();
                }

                return options.Instance;
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
            ExceptionHelper.ValidateAssignableFrom<ILoader>(loaderType);

            services.AddSingleton(typeof(ILoader), loaderType);
            services.AddSingleton(loaderType, p => p.GetServices<ILoader>().First(l => loaderType == l.GetType()));
        }
        #endregion
    }
}
