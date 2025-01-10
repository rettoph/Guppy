namespace Guppy.Tests.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static Lazy<T> ToLazy<T>(this T instance) => new(() => instance);

        public static Lazy<TAs> ToLazy<T, TAs>(this T instance)
            where T : TAs => new(() => instance);
    }
}