using Guppy.Core.Files.Common;
using Guppy.Core.Logging.Common.Constants;

namespace Guppy.Core.Logging.Common.Configurations
{
    public class FileLogMessageSinkConfiguration
    {
        public bool Enabled { get; set; } = false;
        public string OutputTemplate { get; set; } = LoggingConstants.DefaultOutputTemplate;
        public FileLocation? Path { get; set; } = null;
    }
}