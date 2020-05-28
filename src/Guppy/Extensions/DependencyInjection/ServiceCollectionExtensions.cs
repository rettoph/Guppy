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
        public static void AddGame(this ServiceCollection services, Type game, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Game>(game);

            services.AddSingleton(game, factory, priority);
        }
        public static void AddGame<TGame>(this ServiceCollection services, Func<ServiceProvider, TGame> factory, Int32 priority = 0)
            where TGame : Game
        {
            services.AddGame(typeof(TGame), p => factory(p), priority);
        }
        #endregion

        #region Scene Methods
        public static void AddScene(this ServiceCollection services, Type scene, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Scene>(scene);

            services.AddScoped(scene, factory, priority, typeof(Scene));
        }
        public static void AddScene<TScene>(this ServiceCollection services, Func<ServiceProvider, TScene> factory, Int32 priority = 0)
            where TScene : Scene
        {
            services.AddScene(typeof(TScene), p => factory(p), priority);
        }
        #endregion

        #region Entity Methods
        public static void AddEntity(this ServiceCollection services, Type entity, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Entity>(entity);

            services.AddTransient(entity, factory, priority);
        }

        public static void AddEntity<TEntity>(this ServiceCollection services, Func<ServiceProvider, TEntity> factory, Int32 priority = 0)
            where TEntity : Entity
        {
            services.AddEntity(typeof(TEntity), p => factory(p), priority);
        }
        #endregion

        #region Driver Methods
        /// <summary>
        /// Define a new Driver type factory.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="driver"></param>
        /// <param name="factory"></param>
        public static void AddDriver(this ServiceCollection services, Type driver, Func<ServiceProvider, Object> factory, Int32 priority = 0)
        {
            ExceptionHelper.ValidateAssignableFrom<Driver>(driver);

            services.AddTransient(driver, factory, priority);
        }

        /// <summary>
        /// Define a new Driver type factory.
        /// </summary>
        /// <typeparam name="TDriver"></typeparam>
        /// <param name="services"></param>
        /// <param name="factory"></param>
        public static void AddDriver<TDriver>(this ServiceCollection services, Func<ServiceProvider, TDriver> factory, Int32 priority = 0)
            where TDriver : Driver
                => services.AddDriver(typeof(TDriver), p => factory(p), priority);

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
            => services.BindDriver(driven, driver, String.Empty);

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
            }, 5);
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

        public static void AddAndBindDriver<TDriven, TDriver>(this ServiceCollection services, Func<ServiceProvider, TDriver> factory, Int32 priority = 0)
            where TDriver : Driver
            where TDriven : Driven
        {
            services.AddDriver<TDriver>(factory, priority);
            services.BindDriver<TDriven, TDriver>();
        }
        #endregion
    }
}
