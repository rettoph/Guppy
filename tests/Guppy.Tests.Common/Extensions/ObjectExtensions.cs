namespace Guppy.Tests.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static Lazy<T> ToLazy<T>(this T instance)
        {
            return new(() => instance);
        }

        public static Lazy<TAs> ToLazy<T, TAs>(this T instance)
            where T : TAs
        {
            return new(() => instance);
        }

        public static Lazy<TOut> ToLazy<TIn, TOut>(this TIn item, Func<TIn, TOut> factory)
        {
            return new Lazy<TOut>(() => factory(item));
        }
    }
}