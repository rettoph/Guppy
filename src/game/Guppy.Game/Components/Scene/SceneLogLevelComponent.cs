using Guppy.Core.Commands.Common;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common.Components;
using Guppy.Game.Common;
using Serilog.Events;

namespace Guppy.Game.Components.Guppy
{
    [AutoLoad]
    internal class EngineLogLevelComponent : EngineComponent, ICommandSubscriber<LogLevelCommand>
    {
        private readonly ITerminal _terminal;

        private SettingValue<LogEventLevel> _logLevel;

        public EngineLogLevelComponent(ITerminal terminal, ISettingService settings)
        {
            _terminal = terminal;
            _logLevel = settings.GetValue(Settings.LogLevel);

            LogLevelCommand.LoggingLevelSwitch.MinimumLevel = _logLevel;
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
            _terminal.WriteLine(message.Value.ToString(), _terminal.Theme.Get(message.Value.Value));
        }
    }
}
