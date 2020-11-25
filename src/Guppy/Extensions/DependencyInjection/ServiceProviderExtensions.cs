using Guppy.DependencyInjection;
using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        #region Content Methods
        /// <summary>
        /// Automatically return content firectly from the
        /// ServiceProvider's ContentLoader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static T GetContent<T>(this ServiceProvider provider, String handle)
            => provider.GetService<ContentService>().Get<T>(handle);
        /// <summary>
        /// Automatically load content from the internal ContentLoader
        /// and set the value to the recieved instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <param name="content"></param>
        public static void Content<T>(this ServiceProvider provider, String handle, out T content)
            => content = provider.GetContent<T>(handle);
        #endregion
    }
}
