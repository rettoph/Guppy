using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Configurations;
using Guppy.Factories;
using Guppy.Interfaces;

namespace Guppy.Extensions.DependencyInjection
{
    public static class IServiceProviderExtensions
    {
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

        #region GetDrivers Methods
        public static Driver[] GetDrivers(this IServiceProvider provider, Driven driven)
        {
            return provider.GetServices<DriverConfiguration>()
                .Where(dc => dc.DrivenType == driven.GetType())
                .Select(dc => (Driver)ActivatorUtilities.CreateInstance(provider, dc.DriverType, driven))
                .ToArray();
        }
        #endregion
    }
}
