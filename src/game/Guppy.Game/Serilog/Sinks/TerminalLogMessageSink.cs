using System.Text;
using Guppy.Core.Common;
using Guppy.Core.Logging.Common.Enums;
using Guppy.Core.Logging.Common.Sinks;
using Guppy.Game.Common;
using Guppy.Game.Configurations;

namespace Guppy.Game.Serilog.Sinks
{
    public class TerminalLogMessageSink(ITerminal terminal, IConfiguration<TerminalLogMessageSinkConfiguration> configuration) : TextWriter, ILogMessageSink
    {
        private readonly ITerminal _terminal = terminal;

        public string OutputTemplate => configuration.Value.OutputTemplate;

        public LogLevelEnum OutputLogLevel { get; set; }

        public TextWriter OutputWriter => this._terminal.Out;

        public override Encoding Encoding => throw new NotImplementedException();

        // public void Emit(LogEvent logEvent)
        // {
        //     IRef<Color> color = this._terminal.Color;
        //     this._terminal.Color = this._terminal.Theme.Get(logEvent.Level);
        //     this._formatter.Format(logEvent, this._terminal.Out);
        //     this._terminal.Color = color;
        // }

        public override void Write(string? value)
        {
            base.Write(value);
        }

        public override void Write(char value)
        {
            base.Write(value);
        }
    }
}