namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> source)
            where TKey : notnull
        {
            var clone = new Dictionary<TKey, TValue>();

            foreach (KeyValuePair<TKey, TValue> kvp in source)
            {
                clone.Add(kvp.Key, kvp.Value);
            }

            return clone;
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue>? value)
            where TKey : notnull
        {
            if (value is null)
            {
                return source;
            }

            foreach (KeyValuePair<TKey, TValue> kvp in value)
            {
                source[kvp.Key] = kvp.Value;
            }

            return source;
        }
    }
}
