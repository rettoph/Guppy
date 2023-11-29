using Guppy.Attributes;
using Guppy.Commands;
using Guppy.Common;
using Guppy.MonoGame.Commands;
using Guppy.MonoGame.Constants;
using Guppy.Resources.Providers;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components.Guppy
{
    [AutoLoad]
    internal class LogLevelComponent : GuppyComponent, ICommandSubscriber<LogLevelCommand>
    {
        private readonly ITerminal _terminal;
        private readonly Ref<LogEventLevel> _logLevel;

        public LogLevelComponent(ITerminal terminal, ISettingProvider settings)
        {
            _terminal = terminal;
            _logLevel = settings.Get(Settings.LogLevel);

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = _logLevel.Value;
        }

        public void Process(in Guid messageId, in LogLevelCommand message)
        {
            if(message.Value is null)
            {
                _terminal.Write($"Current Log Level: ");
                _terminal.WriteLine(LogLevelCommand.LoggingLevelSwitch.MinimumLevel.ToString(), _terminal.Theme.Get(LogLevelCommand.LoggingLevelSwitch.MinimumLevel));

                return;
            }

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = _logLevel.Value = message.Value.Value;

            _terminal.Write($"Set Log Level: ");
            _terminal.WriteLine(message.Value.Value.ToString(), _terminal.Theme.Get(message.Value.Value));
        }
    }
}
