namespace Guppy.Core.Common.Helpers
{
    public static class EnumHelper
    {
        public static Dictionary<T, TValue> ToDictionary<T, TValue>(params T[] except)
            where T : struct, Enum
            where TValue : new() => ToDictionary(x => new TValue(), except);

        public static Dictionary<T, TValue> ToDictionary<T, TValue>(Func<T, TValue> factory, params T[] except)
            where T : struct, Enum => Enum.GetValues<T>().Except(except).ToDictionary(
                keySelector: x => x,
                elementSelector: x => factory(x));
    }
}