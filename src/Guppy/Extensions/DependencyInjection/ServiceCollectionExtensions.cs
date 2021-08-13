using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using GuppyServiceCollection = Guppy.DependencyInjection.GuppyServiceCollection;
using GuppyServiceProvider = Guppy.DependencyInjection.GuppyServiceProvider;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionLifetimeExtensions
    {
        #region Game Methods
        public static void RegisterGame(
            this GuppyServiceCollection services, 
            Type game, 
            ServiceConfigurationKey? key = default, 
            Func<GuppyServiceProvider, Object> factory = default, 
            Int32 priority = -1)
        {
            ExceptionHelper.ValidateAssignableFrom<Game>(game);

            factory ??= p => Activator.CreateInstance(game);
            services.RegisterTypeFactory(
                type: game, 
                method: factory,
                priority: priority);
            services.RegisterSingleton(
                key: key ?? ServiceConfigurationKey.From(type: game), 
                typeFactory: game,
                baseCacheKey: Guppy.Constants.ServiceConfigurationKeys.GameBaseCacheKey,
                priority: priority);
        }
        public static void RegisterGame<TGame>(
            this GuppyServiceCollection services, 
            ServiceConfigurationKey? key = default, 
            Func<GuppyServiceProvider, TGame> factory = default, 
            Int32 priority = -1)
                where TGame : Game
                    => services.RegisterGame(
                        game: typeof(TGame), 
                        key: key,
                        factory: factory, 
                        priority: priority);
        #endregion

        #region Scene Methods
        public static void RegisterScene(
            this GuppyServiceCollection services, 
            Type scene, 
            ServiceConfigurationKey? key = default, 
            Func<GuppyServiceProvider, Object> factory = default, 
            Int32 priority = -1)
        {
            ExceptionHelper.ValidateAssignableFrom<IScene>(scene);

            factory ??= p => Activator.CreateInstance(scene);
            services.RegisterTypeFactory(
                type: scene, 
                method: factory, 
                priority: priority);
            services.RegisterScoped(
                key: key ?? ServiceConfigurationKey.From(type: scene),
                typeFactory: scene,
                baseCacheKey: Guppy.Constants.ServiceConfigurationKeys.SceneBaseCacheKey,
                priority: priority);
        }
        public static void RegisterScene<TScene>(
            this GuppyServiceCollection services, 
            ServiceConfigurationKey? key = default, 
            Func<GuppyServiceProvider, TScene> factory = null, 
            Int32 priority = -1)
                where TScene : class, IScene
                    => services.RegisterScene(
                        scene: typeof(TScene), 
                        key: key, 
                        factory: factory,
                        priority: priority);
        #endregion
    }
}
