using Guppy.Attributes;
using Guppy.MonoGame.Constants;
using Guppy.Resources;
using Guppy.Resources.Loaders;
using Guppy.Resources.Providers;
using Serilog.Events;

namespace MonoGame.Loaders
{
    [AutoLoad]
    internal sealed class SettingLoader : ISettingLoader
    {
        public void Load(ISettingProvider settings)
        {
            settings.Register(Settings.IsDebugWindowEnabled, false);
            settings.Register(Settings.IsTerminalWindowEnabled, false);

#if DEBUG
            settings.Register(Settings.LogLevel, LogEventLevel.Debug);
#else
            settings.Register(Settings.LogLevel, LogEventLevel.Information);
#endif
        }
    }
}
