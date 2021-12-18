using System;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public static class ServiceCollectionLifetimeExtensions
    {
        #region Game Methods
        public static ServiceConfigurationBuilder<TGame> RegisterGame<TGame>(
            this ServiceProviderBuilder services, 
            String name)
                where TGame : Game
        {
            return services.RegisterService<TGame>(name)
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes<Game, TGame>();
        }
        public static ServiceConfigurationBuilder<TGame> RegisterGame<TGame>(
            this ServiceProviderBuilder services)
                where TGame : Game
        {
            return services.RegisterGame<TGame>(typeof(TGame).FullName);
        }
        public static ServiceConfigurationBuilder<Game> RegisterGame(
            this ServiceProviderBuilder services,
            Type game,
            String name)
        {
            return services.RegisterService<Game>(name)
                .SetLifetime(ServiceLifetime.Singleton)
                .AddCacheNamesBetweenTypes(typeof(Game), game);
        }
        public static ServiceConfigurationBuilder<Game> RegisterGame(
            this ServiceProviderBuilder services,
            Type game)
        {
            return services.RegisterGame(game, game.FullName);
        }
        #endregion

        #region Scene Methods
        public static ServiceConfigurationBuilder<TScene> RegisterScene<TScene>(
            this ServiceProviderBuilder services,
            String name)
                where TScene : Scene
        {
            return services.RegisterService<TScene>(name)
                .SetLifetime(ServiceLifetime.Scoped)
                .AddCacheNamesBetweenTypes<Scene, TScene>();
        }
        public static ServiceConfigurationBuilder<TScene> RegisterScene<TScene>(
            this ServiceProviderBuilder services)
                where TScene : Scene
        {
            return services.RegisterScene<TScene>(typeof(TScene).FullName);
        }
        public static ServiceConfigurationBuilder<Scene> RegisterScene(
            this ServiceProviderBuilder services,
            Type scene,
            String name)
        {
            return services.RegisterService<Scene>(name)
                .SetLifetime(ServiceLifetime.Scoped)
                .AddCacheNamesBetweenTypes(typeof(Scene), scene);
        }
        public static ServiceConfigurationBuilder<Scene> RegisterScene(
            this ServiceProviderBuilder services,
            Type scene)
        {
            return services.RegisterScene(scene, scene.FullName);
        }
        #endregion
    }
}
