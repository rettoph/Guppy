using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Extensions.Linq
{
    public static class ListExtensions
    {
        #if NET462
        public static List<T> Append<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }
        #endif
    }

}
