using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Simply preform an action, useful for 
        /// chaining conversions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Then<T>(this T instance, Action<T> action)
        {
            action(instance);
            return instance;
        }

        /// <summary>
        /// Attempt to cast an object into something else.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static TOut As<TOut>(this Object instance)
            where TOut : class
                => instance as TOut;

        /// <summary>
        /// Manually run a type converter.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="instance"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static TOut As<TIn, TOut>(this TIn instance, Func<TIn, TOut> converter)
            => converter(instance);

        /// <summary>
        /// Convert a single object into an enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(this T instance)
        {
            yield return instance;
        }
    }
}
