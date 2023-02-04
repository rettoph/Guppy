using Guppy.Attributes.Common;
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
            return items.SortBy(x => x.GetType(), defaultOrder);
        }

        public static IOrderedEnumerable<T> SortBy<T>(this IEnumerable<T> items, Func<T, Type> getter, int defaultOrder = 0)
        {
            IList<int> orders = new List<int>();

            return items.OrderBy(item => {
                orders.Clear();

                if(item is ISortable sortable && sortable.GetOrder(typeof(T), out int order))
                {
                    orders.Add(order);
                }

                foreach(var attr in getter(item).GetCustomAttributes())
                {
                    if(attr is SortableAttribute sortableAttr && sortableAttr.Sorts(typeof(T)))
                    {
                        orders.Add(sortableAttr.Order);
                        continue;
                    }

                    if (attr is AutoLoadAttribute autoLoadAttr)
                    {
                        orders.Add(autoLoadAttr.Order);
                        continue;
                    }
                }

                if (orders.Any())
                {
                    return orders.Min();
                }

                return defaultOrder;
            });
        }

        public static IEnumerable<TAs> WhereAs<T, TAs>(this IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                if(item is TAs casted)
                {
                    yield return casted;
                }
            }
        }
    }
}
