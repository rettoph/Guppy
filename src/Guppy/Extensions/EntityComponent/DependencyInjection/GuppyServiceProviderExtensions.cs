using Guppy.Services;
using Guppy.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection
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
        /// <param name="_out"></param>
        public static void Content<T>(this ServiceProvider provider, String handle, out T _out)
            => _out = provider.GetContent<T>(handle);
        #endregion

        #region Color Methods
        /// <summary>
        /// Automatically return a color directly from the
        /// ServiceProvider's ColorService.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Color GetColor(this ServiceProvider provider, String handle)
            => provider.GetService<ColorService>()[handle];
        /// <summary>
        /// Automatically load a color from the internal ColorService
        /// and set the value to the recieved instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <param name="_out"></param>
        public static void Color(this ServiceProvider provider, String handle, out global::Microsoft.Xna.Framework.Color _out)
            => _out = provider.GetColor(handle);
        #endregion

        #region String Methods
        /// <summary>
        /// Automatically return a string directly from the
        /// ServiceProvider's StringService.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static String GetString(this ServiceProvider provider, String handle)
            => provider.GetService<StringService>()[handle];

        /// <summary>
        /// Automatically load a string from the internal StringService
        /// and set the value to the recieved instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <param name="_out"></param>
        public static void String(this ServiceProvider provider, String handle, out String _out)
            => _out = provider.GetString(handle);
        #endregion

        #region Interval Methods
        /// <summary>
        /// REturn the current scope's <see cref="Interval"/> instance
        /// at a requested millisecond interval.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static Interval GetInterval(this ServiceProvider provider, Double milliseconds)
            => provider.GetService<IntervalInvoker>()[milliseconds];
        #endregion


        #region IntervalQueue Methods
        /// <summary>
        /// REturn the current scope's <see cref="IntervalQueue"/> instance
        /// at a requested millisecond interval.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static IntervalQueue GetIntervalQueue(this ServiceProvider provider, Double milliseconds)
            => provider.GetService<IntervalBus>()[milliseconds];
        #endregion
    }
}
