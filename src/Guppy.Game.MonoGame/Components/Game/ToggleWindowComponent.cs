using Guppy.Attributes;
using Guppy.Common;
using Guppy.Game.Input;
using Guppy.Game.MonoGame.Components.Guppy;
using Guppy.Game.MonoGame.Messages;
using Guppy.Resources.Providers;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    internal class ToggleWindowComponent : GlobalComponent, IInputSubscriber<Toggle<GuppyDebugWindowComponent>>, IInputSubscriber<Toggle<TerminalWindowComponent>>
    {
        private Ref<bool> _debug;
        private Ref<bool> _terminal;

        public ToggleWindowComponent(ISettingProvider settings)
        {
            _debug = settings.Get(Settings.IsDebugWindowEnabled);
            _terminal = settings.Get(Settings.IsTerminalWindowEnabled);
        }

        public void Process(in Guid messageId, Toggle<GuppyDebugWindowComponent> message)
        {
            _debug.Value = !_debug.Value;
        }

        public void Process(in Guid messageId, Toggle<TerminalWindowComponent> message)
        {
            _terminal.Value = !_terminal.Value;
        }
    }
}
