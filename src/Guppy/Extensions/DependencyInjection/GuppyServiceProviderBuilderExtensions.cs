using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using DotNetUtils.General.Interfaces;
using Guppy.DependencyInjection.Builders;
using DotNetUtils.DependencyInjection;
using System.Linq;
using DotNetUtils.DependencyInjection.Builders;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionLifetimeExtensions
    {
        #region Game Methods
        public static ServiceConfigurationBuilder<TGame, GuppyServiceProvider> RegisterGame<TGame>(
            this GuppyServiceProviderBuilder services, 
            String name)
                where TGame : Game
        {
            return services.RegisterService<TGame>(name)
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes<Game, TGame>();
        }
        public static ServiceConfigurationBuilder<TGame, GuppyServiceProvider> RegisterGame<TGame>(
            this GuppyServiceProviderBuilder services)
                where TGame : Game
        {
            return services.RegisterGame<TGame>(typeof(TGame).FullName);
        }
        public static ServiceConfigurationBuilder<Game, GuppyServiceProvider> RegisterGame(
            this GuppyServiceProviderBuilder services,
            Type game,
            String name)
        {
            return services.RegisterService<Game>(name)
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes(typeof(Game), game);
        }
        public static ServiceConfigurationBuilder<Game, GuppyServiceProvider> RegisterGame(
            this GuppyServiceProviderBuilder services,
            Type game)
        {
            return services.RegisterGame(game, game.FullName);
        }
        #endregion

        #region Scene Methods
        public static ServiceConfigurationBuilder<TScene, GuppyServiceProvider> RegisterScene<TScene>(
            this GuppyServiceProviderBuilder services,
            String name)
                where TScene : Scene
        {
            return services.RegisterService<TScene>(name)
                .SetLifetime(ServiceLifetime.Scoped)
                .AddCacheNamesBetweenTypes<Scene, TScene>();
        }
        public static ServiceConfigurationBuilder<TScene, GuppyServiceProvider> RegisterScene<TScene>(
            this GuppyServiceProviderBuilder services)
                where TScene : Scene
        {
            return services.RegisterScene<TScene>(typeof(TScene).FullName);
        }
        public static ServiceConfigurationBuilder<Scene, GuppyServiceProvider> RegisterScene(
            this GuppyServiceProviderBuilder services,
            Type scene,
            String name)
        {
            return services.RegisterService<Scene>(name)
                .SetLifetime(ServiceLifetime.Scoped)
                .AddCacheNamesBetweenTypes(typeof(Scene), scene);
        }
        public static ServiceConfigurationBuilder<Scene, GuppyServiceProvider> RegisterScene(
            this GuppyServiceProviderBuilder services,
            Type scene)
        {
            return services.RegisterScene(scene, scene.FullName);
        }
        #endregion
    }
}
