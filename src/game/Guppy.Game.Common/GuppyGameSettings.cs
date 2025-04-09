using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Assets.Common;

namespace Guppy.Game.Common
{
    public static class GuppyGameSettings
    {
#if DEBUG
        private const LogLevelEnum _defaultLogLevel = LogLevelEnum.Debug;
#else
        private const LogLevelEnum _defaultLogLevel = LogLevelEnum.Information;
#endif

        public static readonly Setting<LogLevelEnum> LogLevel = Setting<LogLevelEnum>.Get(nameof(LogLevel), "Log level. Valid Values: Verbose, Debug, Information, Warning, Fatal.", _defaultLogLevel);
    }
}