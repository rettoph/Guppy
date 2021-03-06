﻿using Guppy.DependencyInjection;
using Guppy.Services;
using Microsoft.Xna.Framework;
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
            => provider.GetService<Content>().Get<T>(handle);

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
            => provider.GetService<Colors>()[handle];
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

        #region Color Methods
        /// <summary>
        /// Automatically return a string directly from the
        /// ServiceProvider's StringService.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static String GetString(this ServiceProvider provider, String handle)
            => provider.GetService<Strings>()[handle];
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
    }
}
