using Guppy.Core.Logging.Common.Enums;

namespace Guppy.Core.Logging.Common.Sinks
{
    public interface ILogMessageSink
    {
        bool Enabled { get; }
        string OutputTemplate { get; }
        LogLevelEnum OutputLogLevel { get; set; }
        TextWriter OutputWriter { get; }
    }
}
