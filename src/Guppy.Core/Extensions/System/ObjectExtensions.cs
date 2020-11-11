using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class ObjectExtensions
    {
        public static T Then<T>(this T instance, Action<T> action)
        {
            action(instance);
            return instance;
        }

        public static TOut As<TOut>(this Object instance)
            where TOut : class
                => instance as TOut;

        public static TOut As<TIn, TOut>(this TIn instance, Func<TIn, TOut> converter)
            => converter(instance);
    }
}
