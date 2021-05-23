using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Services;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ServiceCollection = Guppy.DependencyInjection.ServiceCollection;
using ServiceProvider = Guppy.DependencyInjection.ServiceProvider;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionLifetimeExtensions
    {
        #region Game Methods
        public static void RegisterGame(
            this ServiceCollection services, 
            Type game, 
            ServiceConfigurationKey? key = default, 
            Func<ServiceProvider, Object> factory = default, 
            Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Game>(game);

            factory ??= p => ActivatorUtilities.CreateInstance(p, game);
            services.RegisterTypeFactory(
                type: game, 
                method: factory,
                priority: priority);
            services.RegisterSingleton(
                key: key ?? ServiceConfigurationKey.From(type: game), 
                priority: priority,
                baseLookupType: typeof(Game));
        }
        public static void RegisterGame<TGame>(
            this ServiceCollection services, 
            ServiceConfigurationKey? key = default, 
            Func<ServiceProvider, TGame> factory = default, 
            Int32 priority = 0)
                where TGame : Game
                    => services.RegisterGame(
                        game: typeof(TGame), 
                        key: key,
                        factory: factory, 
                        priority: priority);
        #endregion

        #region Scene Methods
        public static void RegisterScene(
            this ServiceCollection services, 
            Type scene, 
            ServiceConfigurationKey? key = default, 
            Func<ServiceProvider, Object> factory = default, 
            Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<IScene>(scene);

            factory ??= p => ActivatorUtilities.CreateInstance(p, scene);
            services.RegisterTypeFactory(
                type: scene, 
                method: factory, 
                priority: priority);
            services.RegisterScoped(
                key: key  ?? ServiceConfigurationKey.From(type: scene), 
                priority: priority,
                baseLookupType: typeof(IScene));
        }
        public static void RegisterScene<TScene>(
            this ServiceCollection services, 
            ServiceConfigurationKey? key = default, 
            Func<ServiceProvider, TScene> factory = null, 
            Int32 priority = 0)
                where TScene : class, IScene
                    => services.RegisterScene(
                        scene: typeof(TScene), 
                        key: key, 
                        factory: factory,
                        priority: priority);
        #endregion
    }
}
