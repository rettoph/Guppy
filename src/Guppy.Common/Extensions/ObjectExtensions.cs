using Guppy.Common.Attributes;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
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
        public static TOut As<TOut>(this object instance)
            where TOut : class
                => (instance as TOut)!;

        /// <summary>
        /// Dynamically preform the cast via reflection
        /// </summary>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic As(this object o, Type type)
        {
            var methodInfo = typeof(ObjectExtensions).GetMethod(nameof(As), BindingFlags.Static | BindingFlags.Public);
            var genericArguments = new[] { type };
            var genericMethodInfo = methodInfo?.MakeGenericMethod(genericArguments);
            return genericMethodInfo?.Invoke(null, new[] { o })!;
        }

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

        public static int GetOrderAs<T>(this T item, Type type)
        {
            if (item is ISortable sortable && sortable.GetOrder(type, out int order))
            {
                return order;
            }

            return item!.GetType().GetCustomAttributes()
                .OfType<SortableAttribute>()
                .Where(attr => attr.Sorts(type))
                .DefaultIfEmpty()
                .Min(attr => attr?.Order ?? 0);
        }

        public static int GetOrder<T>(this T item)
        {
            return item.GetOrderAs(typeof(T));
        }
    }
}
