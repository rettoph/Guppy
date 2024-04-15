using Guppy.Core.Common.Collections;

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

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, T item)
        {
            return collection.Concat([item]);
        }
    }
}
