using Guppy.Core.Common;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Guppy.Game.Serilog.Sinks
{
    public class TerminalSink(ITerminal terminal, string outputTemplate) : ILogEventSink
    {
        private readonly ITextFormatter _formatter = new MessageTemplateTextFormatter(outputTemplate, null);
        private readonly ITerminal _terminal = terminal;

        public void Emit(LogEvent logEvent)
        {
            IRef<Color> color = _terminal.Color;
            _terminal.Color = _terminal.Theme.Get(logEvent.Level);
            _formatter.Format(logEvent, _terminal.Out);
            _terminal.Color = color;
        }
    }
}
