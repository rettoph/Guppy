using Guppy.DependencyInjection;
using Guppy.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        #region Game Methods
        public static void AddGame(this ServiceCollection services, Type game, Func<ServiceProvider, Object> factory)
        {
            ExceptionHelper.ValidateAssignableFrom<Game>(game);

            services.AddSingleton(game, factory);
        }
        public static void AddGame<TGame>(this ServiceCollection services, Func<ServiceProvider, TGame> factory)
            where TGame : Game
        {
            services.AddGame(typeof(TGame), p => factory(p));
        }
        #endregion

        #region Scene Methods
        public static void AddScene(this ServiceCollection services, Type scene, Func<ServiceProvider, Object> factory)
        {
            ExceptionHelper.ValidateAssignableFrom<Scene>(scene);

            services.AddScoped(scene, factory, 0, typeof(Scene));
        }
        public static void AddScene<TScene>(this ServiceCollection services, Func<ServiceProvider, TScene> factory)
            where TScene : Scene
        {
            services.AddScene(typeof(TScene), p => factory(p));
        }
        #endregion

        #region Entity Methods
        public static void AddEntity(this ServiceCollection services, Type entity, Func<ServiceProvider, Object> factory)
        {
            ExceptionHelper.ValidateAssignableFrom<Entity>(entity);

            services.AddTransient(entity, factory);
        }

        public static void AddEntity<TEntity>(this ServiceCollection services, Func<ServiceProvider, TEntity> factory)
            where TEntity : Entity
        {
            services.AddEntity(typeof(TEntity), p => factory(p));
        }
        #endregion

        #region Driver Methods
        /// <summary>
        /// Define a new Driver type factory.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driver"></param>
        /// <param name="factory"></param>
        public static void AddDriver(this ServiceCollection services, Type driver, Func<ServiceProvider, Object> factory)
        {
            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            services.AddTransient(driver, factory);
        }

        /// <summary>
        /// Define a new Driver type factory.
        /// </summary>
        /// <typeparam name="TDriver"></typeparam>
        /// <param name="services"></param>
        /// <param name="factory"></param>
        public static void AddDriver<TDriver>(this ServiceCollection services, Func<ServiceProvider, TDriver> factory)
            where TDriver : Driver
                => services.AddDriver(typeof(TDriver), p => factory(p));

        /// <summary>
        /// Bind a Driver type to a recieved Driven type. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver(this ServiceCollection services, Type driven, Type driver)
        {
            ExceptionHelper.ValidateAssignableFrom<Driven>(driven);
            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            services.AddConfiguration(driven, String.Empty, (i, p, f) =>
            {
                ((Driven)i).AddDriver(driver);
                return i;
            });
        }

        /// <summary>
        /// Bind a Driver type to a recieved Driven type. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver<TDriven, TDriver>(this ServiceCollection services)
            where TDriven : Driven
            where TDriver : Driver
                => services.BindDriver(typeof(TDriven), typeof(TDriver));

        /// <summary>
        /// Bind a Driver type to a recieved Driven type configuration. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created with the 
        /// recieved configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver(this ServiceCollection services, Type driven, Type driver, String configuration)
        {
            ExceptionHelper.ValidateAssignableFrom<Driven>(driven);
            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            services.AddConfiguration(driven, configuration, (i, p, f) =>
            {
                ((Driven)i).AddDriver(driver);
                return i;
            });
        }

        /// <summary>
        /// Bind a Driver type to a recieved Driven type configuration. 
        /// 
        /// This will automatically create a new instance of driver
        /// for every driven instance created with the 
        /// recieved configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driven"></param>
        /// <param name="driver"></param>
        public static void BindDriver<TDriven, TDriver>(this ServiceCollection services, String configuration)
            where TDriven : Driven
            where TDriver : Driver
                => services.BindDriver(typeof(TDriven), typeof(TDriver));
        #endregion
    }
}
