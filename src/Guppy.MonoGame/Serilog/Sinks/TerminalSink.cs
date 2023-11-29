using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Serilog.Sinks
{
    public class TerminalSink : ILogEventSink
    {
        private readonly ITextFormatter _formatter;
        private readonly ITerminal _terminal;

        public TerminalSink(ITerminal terminal, string outputTemplate)
        {
            _terminal = terminal;
            _formatter = new MessageTemplateTextFormatter(outputTemplate, null);
        }

        public void Emit(LogEvent logEvent)
        {
            var color = _terminal.Color;
            _terminal.Color = _terminal.Theme.Get(logEvent.Level);
            _formatter.Format(logEvent, _terminal.Out);
            _terminal.Color = color;
        }
    }
}
