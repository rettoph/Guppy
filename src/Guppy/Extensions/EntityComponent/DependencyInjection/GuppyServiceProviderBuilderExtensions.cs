using System;

namespace Guppy.EntityComponent.DependencyInjection.Builders
{
    public static class ServiceCollectionLifetimeExtensions
    {
        #region Game Methods
        public static ServiceConfigurationBuilder<TGame> RegisterGame<TGame>(
            this ServiceProviderBuilder services, 
            Type type)
                where TGame : Game
        {
            return services.RegisterService<TGame>(type)
                .SetLifetime(ServiceLifetime.Singleton)
                .AddAllAliases<Game>();
        }
        public static ServiceConfigurationBuilder<TGame> RegisterGame<TGame>(
            this ServiceProviderBuilder services)
                where TGame : Game
        {
            return services.RegisterGame<TGame>(typeof(TGame));
        }
        public static ServiceConfigurationBuilder<Game> RegisterGame(
            this ServiceProviderBuilder services,
            Type game)
        {
            return services.RegisterGame<Game>(game);
        }
        #endregion

        #region Scene Methods
        public static ServiceConfigurationBuilder<TScene> RegisterScene<TScene>(
            this ServiceProviderBuilder services,
            Type type)
                where TScene : Scene
        {
            return services.RegisterService<TScene>(type)
                .SetLifetime(ServiceLifetime.Scoped)
                .AddAllAliases<Scene>();
        }
        public static ServiceConfigurationBuilder<TScene> RegisterScene<TScene>(
            this ServiceProviderBuilder services)
                where TScene : Scene
        {
            return services.RegisterScene<TScene>(typeof(TScene));
        }
        public static ServiceConfigurationBuilder<Scene> RegisterScene(
            this ServiceProviderBuilder services,
            Type scene)
        {
            return services.RegisterScene<Scene>(scene);
        }
        #endregion
    }
}
