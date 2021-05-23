using Guppy.IO.Services;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.IO.Utilities
{
    public class log4netTerminalAppender : IAppender
    {
        private Terminal _terminal;

        public Dictionary<Level, Color> Colors { get; private set; }

        public PatternLayout PatternLayout = new PatternLayout();

        public String Name { get; set; }

        public log4netTerminalAppender(Terminal terminal, PatternLayout layout, params (Level, Color)[] colors)
        {
            _terminal = terminal;

            this.PatternLayout = layout;
            this.PatternLayout.ActivateOptions();

            this.Colors = colors.ToDictionary(
                keySelector: kvp => kvp.Item1,
                elementSelector: kvp => kvp.Item2);
        }

        public void Close()
        {
            _terminal = null;

            this.PatternLayout = null;
            this.Colors = null;
        }

        public void DoAppend(LoggingEvent loggingEvent)
            => _terminal.Write(this.PatternLayout.Format(loggingEvent), this.Colors[loggingEvent.Level]);
    }
}
