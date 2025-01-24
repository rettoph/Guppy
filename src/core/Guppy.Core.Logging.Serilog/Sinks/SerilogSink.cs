using Guppy.Core.Logging.Common.Sinks;
using Guppy.Core.Logging.Serilog.Extensions;
using Serilog.Core;
using Serilog.Events;
using Serilog.Templates;

namespace Guppy.Core.Logging.Serilog.Sinks
{
    public class SerilogSink(ILogMessageSink sink) : ILogEventSink
    {
        private readonly ILogMessageSink _sink = sink;
        private readonly ExpressionTemplate _template = new(sink.OutputTemplate);

        public void Emit(LogEvent logEvent)
        {
            this._sink.OutputLogLevel = logEvent.Level.ToLogLevelEnum();
            this._template.Format(logEvent, this._sink.OutputWriter);
        }
    }
}
