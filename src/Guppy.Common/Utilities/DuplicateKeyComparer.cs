namespace Guppy.Common.Utilities
{
    public sealed class DuplicateKeyComparer<TKey> : IComparer<TKey>
        where TKey : IComparable
    {
        public int Compare(TKey? x, TKey? y)
        {
            if (x is null || y is null)
            {
                return -1;
            }

            int result = x.CompareTo(y);

            if (result == 0)
                return 1; // Handle equality as being greater. Note: this will break Remove(key) or
            else          // IndexOfKey(key) since the comparer never returns 0 to signal key equality
                return result;
        }
    }
}
