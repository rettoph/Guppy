using Guppy.Collections;
using Guppy.Factories;
using Guppy.Utilities;
using Guppy.Utilities.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        #region Scoped Methods
        /// <summary>
        /// Add an entity and scope it, ensuring that it can be loaded
        /// via dependency injection. Note, Manually calling the EntityCollection.Create
        /// method will allow users to create multiple instances.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="services"></param>
        /// <param name="handle"></param>
        /// <param name="setup"></param>
        /// <param name="create"></param>
        public static void AddScoped<TEntity>(this IServiceCollection services, String handle, Action<TEntity> setup = null, Action<TEntity> create = null)
            where TEntity : Entity
        {
            services.AddTransient<TEntity>(p =>
            {
                var scope = p.GetRequiredService<ScopeOptions>();
                TEntity instance;
                if ((instance = scope.Get<TEntity>()) == null)
                { // Create a new instance of the requested entity...
                    instance = p.GetRequiredService<EntityCollection>().Create(handle, setup, create);
                    scope.Set<TEntity>(instance);
                }

                return instance;
            });
        }

        /// <summary>
        /// Add a custom scoped Creatable complete with a custom factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="services"></param>
        public static void AddScoped<T, TFactory>(this IServiceCollection services, Action<T> setup = null, Action<T> create = null)
            where T : Creatable
            where TFactory : Factory<T>
        {
            services.AddScoped<T, T, TFactory>(setup, create);
        }
        /// <summary>
        /// Add a custom scoped Creatable complete with a custom factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="services"></param>
        /// <param name="setup"></param>
        /// <param name="create"></param>
        public static void AddScoped<T, TImplementation, TFactory>(this IServiceCollection services, Action<TImplementation> setup = null, Action<TImplementation> create = null)
            where T : TImplementation
            where TImplementation : Creatable
            where TFactory : Factory<TImplementation>
        {
            services.AddTransient<T>(p =>
            {
                var scope = p.GetRequiredService<ScopeOptions>();
                TImplementation instance;
                if ((instance = scope.Get<TImplementation>()) == null)
                { // Create a new instance of the requested entity...
                    instance = p.GetRequiredService<TFactory>().Build(setup, create);
                    scope.Set<TImplementation>(instance);
                }

                // Ensure that the current scope value is assignable...
                ExceptionHelper.ValidateAssignableFrom(typeof(T), instance.GetType());

                return instance as T;
            });
        }
        #endregion

        #region Transient Methods
        /// <summary>
        /// Add a custom scoped Creatable complete with a custom factory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TFactory"></typeparam>
        /// <param name="services"></param>
        public static void AddTransient<T, TFactory>(this IServiceCollection services, Action<T> setup = null, Action<T> create = null)
            where T : Creatable
            where TFactory : Factory<T>
        {
            services.AddTransient<T>(p => p.GetRequiredService<TFactory>().Build<T>(setup, create));
        }
        #endregion
    }
}
