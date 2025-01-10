using Guppy.Core.Common;
using Guppy.Game.Common;
using Microsoft.Xna.Framework;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Guppy.Game.Serilog.Sinks
{
    public class TerminalSink(ITerminal terminal, string outputTemplate) : ILogEventSink
    {
        private readonly MessageTemplateTextFormatter _formatter = new(outputTemplate, null);
        private readonly ITerminal _terminal = terminal;

        public void Emit(LogEvent logEvent)
        {
            IRef<Color> color = this._terminal.Color;
            this._terminal.Color = this._terminal.Theme.Get(logEvent.Level);
            this._formatter.Format(logEvent, this._terminal.Out);
            this._terminal.Color = color;
        }
    }
}