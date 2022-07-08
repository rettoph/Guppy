using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Helpers
{
    public static class ActivatorUtilitiesHelper
    {
        private static Type[] EmptyTypeArray = Array.Empty<Type>();
        private static object[] EmptyObjectArray = Array.Empty<object>();

        public static Func<IServiceProvider, T> BuildFactory<T>()
            where T : class
        {
            ObjectFactory factory = ActivatorUtilities.CreateFactory(typeof(T), EmptyTypeArray);

            T Method(IServiceProvider provider)
            {
                return (T)factory.Invoke(provider, EmptyObjectArray);
            }

            return Method;
        }

        public static Func<IServiceProvider, TArg1, T> BuildFactory<TArg1, T>()
            where T : class
        {
            Type[] args = new Type[] { typeof(TArg1) };
            ObjectFactory factory = ActivatorUtilities.CreateFactory(typeof(T), args);
            object[] buffer = new object[1];

            T Method(IServiceProvider provider, TArg1 arg1)
            {
                buffer[0] = arg1;
                return (T)factory.Invoke(provider, buffer);
            }

            return Method;
        }
    }
}
