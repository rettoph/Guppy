namespace Guppy.Tests.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static Lazy<T> ToLazy<T>(this T instance)
        {
            return new Lazy<T>(() => instance);
        }
    }
}
