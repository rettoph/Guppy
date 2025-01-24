using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Core.Logging.Common.Configurations
{
    public class FileMessageSinkConfiguration
    {
        public bool Enabled { get; set; } = false;
        public string OutputTemplate { get; set; } = LoggingConstants.DefaultOutputTemplate;
        public string? Path { get; set; } = null;
    }
}