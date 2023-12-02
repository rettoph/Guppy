using Guppy.Attributes;
using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;
using Serilog.Events;

namespace Guppy.Game.Loaders
{
    [AutoLoad]
    internal sealed class SettingLoader : ISettingLoader
    {
        public void Load(ISettingProvider settings)
        {
#if DEBUG
            settings.Register(Settings.LogLevel, LogEventLevel.Debug);
#else
            settings.Register(Settings.LogLevel, LogEventLevel.Information);
#endif
        }
    }
}
