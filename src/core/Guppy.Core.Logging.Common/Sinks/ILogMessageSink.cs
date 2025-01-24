using Guppy.Core.Logging.Common.Enums;

namespace Guppy.Core.Logging.Common.Sinks
{
    public interface ILogMessageSink
    {
        string OutputTemplate { get; }
        LogLevelEnum OutputLogLevel { get; set; }
        TextWriter OutputWriter { get; }
    }
}
