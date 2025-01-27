using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Game.Common.Configurations
{
    public class TerminalLogMessageSinkConfiguration
    {
        public string OutputTemplate { get; set; } = LoggingConstants.DefaultOutputTemplate;
        public bool Enabled { get; set; } = false;
    }
}
