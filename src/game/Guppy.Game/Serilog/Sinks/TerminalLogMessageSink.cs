using Guppy.Core.Common;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Logging.Common.Sinks;
using Guppy.Game.Common;
using Guppy.Game.Common.Configurations;

namespace Guppy.Game.Serilog.Sinks
{
    public class TerminalLogMessageSink(ITerminal terminal, IConfiguration<TerminalLogMessageSinkConfiguration> configuration) : ILogMessageSink
    {
        private readonly ITerminal _terminal = terminal;

        public bool Enabled { get; private set; } = configuration.Value.Enabled;
        public string OutputTemplate => configuration.Value.OutputTemplate;

        public LogLevelEnum OutputLogLevel { get; set; }

        public TextWriter OutputWriter => this._terminal.Out;
    }
}