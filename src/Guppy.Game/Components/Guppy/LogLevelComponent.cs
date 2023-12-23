using Guppy.Attributes;
using Guppy.Commands;
using Guppy.Common;
using Guppy.Game.Common;
using Guppy.Resources.Providers;
using Serilog.Events;

namespace Guppy.Game.Components.Guppy
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

        public void Process(in Guid messageId, LogLevelCommand message)
        {
            if (message.Value is null)
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
