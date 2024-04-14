using Guppy.Game.Common;
using Guppy.Game.Serilog.Sinks;
using Serilog;
using Serilog.Configuration;

namespace Guppy.Game.Extensions.Serilog
{
    public static class LoggerConfigurationExtensions
    {
        internal const string DefaultTerminalOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static LoggerConfiguration Terminal(
            this LoggerSinkConfiguration loggerConfiguration,
            ITerminal terminal,
            string outputTemplate = DefaultTerminalOutputTemplate)
        {
            return loggerConfiguration.Sink(new TerminalSink(terminal, outputTemplate));
        }
    }
}
