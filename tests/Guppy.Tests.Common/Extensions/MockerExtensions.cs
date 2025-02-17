using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;

namespace Guppy.Tests.Common.Extensions
{
    public static class MockerExtensions
    {
        public static Mocker<ISettingService> Setup<T>(this Mocker<ISettingService> mocker, Setting<T> setting, T value)
            where T : notnull
        {
            mocker.Setup(x => x.GetValue(setting), setting.MockValue(value));

            return mocker;
        }
    }
}
