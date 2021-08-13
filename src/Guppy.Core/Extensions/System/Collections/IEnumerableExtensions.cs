using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.System.Collections
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        
            return source;
        }
        
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, Int32> action)
        {
            Int32 index = 0;
            foreach (T item in source)
                action(item, index++);
        
            return source;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer ??= Comparer<TKey>.Default;

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// Attempt to run <paramref name="func"/> within
        /// the LINQ <see cref="IEnumerable{T}.Aggregate"/>
        /// method. If no items exist within the recieved <paramref name="source"/>
        /// then simply return the <paramref name="fallback"/> value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static T TryAggregate<T>(this IEnumerable<T> source, Func<T, T, T> func, T fallback = default)
        {
            if (source.Any())
                return source.Aggregate(func);
            else
                return fallback;
        }

        public static T TryGetElementAt<T>(this IEnumerable<T> source, Int32 index, Int32 fallback = 0)
        {
            return source.ElementAtOrDefault(index) ?? source.ElementAt(fallback);
        }

        public static T Random<T>(this IEnumerable<T> source, Random rand = default)
        {
            rand ??= new Random();
            var skip = (int)(rand.NextDouble() * source.Count());
            return source.Skip(skip).Take(1).First();
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, params T[] items)
            => source.Concat(items.AsEnumerable());

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvps)
            => kvps.ToDictionary(
                keySelector: kvp => kvp.Key,
                elementSelector: kvp => kvp.Value);
    }
}
