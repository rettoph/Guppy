using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;

namespace Guppy.Tests.Common.Extensions
{
    public static class MockerExtensions
    {
        public static Mocker<ISettingService> SetupReturn<T>(this Mocker<ISettingService> mocker, Setting<T> setting, T value)
            where T : notnull
        {
            mocker.SetupReturn(x => x.GetValue(setting), setting.MockValue(value));

            return mocker;
        }
    }
}
