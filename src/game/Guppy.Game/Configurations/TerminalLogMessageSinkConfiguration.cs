using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Game.Configurations
{
    public class TerminalLogMessageSinkConfiguration
    {
        public string OutputTemplate { get; set; } = LoggingConstants.DefaultOutputTemplate;
    }
}
