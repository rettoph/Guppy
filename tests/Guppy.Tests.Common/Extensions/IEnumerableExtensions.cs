namespace Guppy.Tests.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Lazy<IEnumerable<TOut>> ToLazy<TIn, TOut>(this IEnumerable<TIn> items, Func<TIn, TOut> factory)
        {
            return new Lazy<IEnumerable<TOut>>(() => items.Select(factory));
        }
    }
}
