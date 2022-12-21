using Guppy.Common;
using Guppy.Common.Attributes;
using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue>(
            this IEnumerable<TValue> values,
            Func<TValue, TKey1> keySelector1,
            Func<TValue, TKey2> keySelector2)
                where TKey1 : notnull
                where TKey2 : notnull
        {
            var kkvs = values.Select(x => (keySelector1(x), keySelector2(x), x)).ToList();

            return new DoubleDictionary<TKey1, TKey2, TValue>(kkvs);
        }

        public static DoubleDictionary<TKey1, TKey2, TValue> ToDoubleDictionary<TKey1, TKey2, TValue, TInput>(
            this IEnumerable<TInput> input,
            Func<TInput, TKey1> keySelector1,
            Func<TInput, TKey2> keySelector2,
            Func<TInput, TValue> valueSelector)
                where TKey1 : notnull
                where TKey2 : notnull
        {
            var kkvps = input.Select(x => (keySelector1(x), keySelector2(x), valueSelector(x)));

            return new DoubleDictionary<TKey1, TKey2, TValue>(kkvps);
        }

        public static IOrderedEnumerable<T> Sort<T>(this IEnumerable<T> items, int defaultOrder = 0)
        {
            return items.OrderBy(item => {
                var attributeOrders = item?.GetType().GetCustomAttributes()
                    .Where(attr =>
                    {
                        if (attr is SortableAttribute sortable)
                        {
                            return sortable.Sorts(typeof(T));
                        }

                        return false;
                    })
                    .Select(attr => ((SortableAttribute)attr).Order) ?? Enumerable.Empty<int>();

                if(attributeOrders.Any())
                {
                    return attributeOrders.Last();
                }

                return defaultOrder;
            });
        }
    }
}
