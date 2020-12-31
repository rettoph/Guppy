using Guppy.Extensions.System.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    public static class EnumHelper
    {
        private static Dictionary<Type, Array> _values = new Dictionary<Type, Array>();

        public static T[] GetValues<T>()
            where T : Enum
                => (_values.ContainsKey(typeof(T)) ? _values[typeof(T)] : _values[typeof(T)] = Enum.GetValues(typeof(T))) as T[];

        public static void ForEach<T>(Action<T> action)
            where T : Enum
                => EnumHelper.GetValues<T>().ForEach(action);

        public static Int32 Count<T>()
            where T : Enum
                => EnumHelper.GetValues<T>().Length;
    }
}
