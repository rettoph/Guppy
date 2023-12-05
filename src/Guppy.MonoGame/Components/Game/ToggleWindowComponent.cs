﻿using Guppy.Attributes;
using Guppy.Common;
using Guppy.Game.ImGui.Styling;
using Guppy.Game.ImGui;
using Guppy.Input;
using Guppy.Game;
using Guppy.MonoGame.Messages;
using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Components.Guppy;

namespace Guppy.MonoGame.Components.Game
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
