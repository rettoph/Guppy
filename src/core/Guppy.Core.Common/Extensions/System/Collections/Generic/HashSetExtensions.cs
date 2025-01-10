namespace Guppy.Core.Common.Extensions.System.Collections.Generic
{
    public static class HashSetExtensions
    {
        public static int AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> items)
        {
            int count = 0;

            foreach (T item in items)
            {
                if (hashSet.Add(item) == true)
                {
                    count++;
                }
            }

            return count;
        }
    }
}