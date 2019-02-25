using Guppy.Configurations;
using Guppy.Factories;
using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;

namespace Guppy.Extensions
{
    /// <summary>
    /// Add custom game extensions to the main
    /// ServiceCollection class.
    /// </summary>
    public static class DependencyInjection
    {
        #region Add Methods
        public static void AddScene<TScene>(this ServiceCollection collection)
            where TScene : Scene
        {
            collection.AddScoped<TScene>(SceneFactory<TScene>.BuildFactory<TScene>().Create);
        }

        public static void AddLayer<TLayer>(this ServiceCollection collection)
            where TLayer : Layer
        {
            var factory = LayerFactory<TLayer>.BuildFactory<TLayer>();
            collection.AddSingleton<LayerFactory<TLayer>>(factory); // Add the new factory (for future custom creation reference)
            collection.AddScoped<TLayer>(factory.Create); // Add the factory's create method as the default constructor
        }

        public static void AddLoader<TLoader>(this ServiceCollection collection)
            where TLoader : class, ILoader
        {
            // Add the loader as a singleton
            collection.AddSingleton<ILoader, TLoader>();
        }
        #endregion

        #region Get Methods
        #region GetScene Methods
        public static TScene GetScene<TScene>(this IServiceProvider provider)
            where TScene : Scene
        {
            return provider.GetService<TScene>();
        }
        #endregion

        #region GetLayer Methods
        public static TLayer GetLayer<TLayer>(this IServiceProvider provider)
            where TLayer : Layer
        {
            return provider.GetService<TLayer>();
        }

        public static TLayer GetLayer<TLayer>(this IServiceProvider provider, LayerConfiguration configuration)
            where TLayer : Layer
        {
            var factory = provider.GetRequiredService<LayerFactory<TLayer>>();

            return factory.Create(provider, configuration);
        }

        public static TLayer GetLayer<TLayer>(this IServiceProvider provider, UInt16 minDepth = 0, UInt16 maxDepth = 0, UInt16 updateOrder = 0, UInt16 drawOrder = 0)
            where TLayer : Layer
        {
            return provider.GetLayer<TLayer>(new LayerConfiguration(minDepth, maxDepth, updateOrder, drawOrder));
        }
        #endregion

        #region GetLoader Methods
        public static IEnumerable<ILoader> GetLoaders(this IServiceProvider provider)
        {
            return provider.GetServices<ILoader>();
        }

        public static TLoader GetLoader<TLoader>(this IServiceProvider provider)
            where TLoader : class, ILoader
        {
            var loaderType = typeof(TLoader);

            return provider.GetLoaders()
                .First(l => l.GetType() == loaderType) as TLoader;
        }
        #endregion
        #endregion
    }
}
