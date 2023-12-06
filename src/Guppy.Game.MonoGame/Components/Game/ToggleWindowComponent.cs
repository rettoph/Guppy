using Guppy.Attributes;
using Guppy.Common;
using Guppy.Game.ImGui.Styling;
using Guppy.Game.ImGui;
using Guppy.Game.Input;
using Guppy.Game;
using Guppy.Game.MonoGame.Messages;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Game.MonoGame.Constants;
using Guppy.Game.MonoGame.Components.Guppy;

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
