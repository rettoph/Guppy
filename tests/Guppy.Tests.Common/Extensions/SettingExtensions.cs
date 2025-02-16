using Guppy.Core.Resources.Common;

namespace Guppy.Tests.Common.Extensions
{
    public static class SettingExtensions
    {
        public static SettingValue<T> MockValue<T>(this Setting<T> setting, T value)
            where T : notnull
        {
            return new SettingValue<T>(setting, value);
        }
    }
}
