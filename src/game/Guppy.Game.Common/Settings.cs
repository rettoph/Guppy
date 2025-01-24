using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Resources.Common;

namespace Guppy.Game.Common
{
    public static class Settings
    {
#if DEBUG
        private const LogLevelEnum _defaultLogLevel = LogLevelEnum.Debug;
#else
        private const LogLevelEnum _defaultLogLevel = LogLevelEnum.Information;
#endif

        public static readonly Setting<LogLevelEnum> LogLevel = Setting<LogLevelEnum>.Get(nameof(LogLevel), "Log level. Valid Values: Verbose, Debug, Information, Warning, Fatal.", _defaultLogLevel);
    }
}