using Guppy.Core.Logging.Common.Enums;

namespace Guppy.Core.Common.Configurations
{
    public class LogLevelConfiguration
    {
#if DEBUG
        public LogLevelEnum Default { get; set; } = LogLevelEnum.Debug;
#else
        public LogLevelEnum Default { get; set; } = LogLevelEnum.Information;
#endif

        public Dictionary<string, LogLevelEnum?> Overrides { get; set; } = [];
    }
}