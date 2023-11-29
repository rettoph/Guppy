using Guppy.MonoGame.Serilog.Sinks;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Extensions.Serilog
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
