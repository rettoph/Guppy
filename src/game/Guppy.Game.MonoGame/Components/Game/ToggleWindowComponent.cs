﻿using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Game.Input.Common;
using Guppy.Game.MonoGame.Components.Guppy;
using Guppy.Game.MonoGame.Messages;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    internal class ToggleWindowComponent : GlobalComponent, IInputSubscriber<Toggle<GuppyDebugWindowComponent>>, IInputSubscriber<Toggle<TerminalWindowComponent>>
    {
        public void Process(in Guid messageId, Toggle<GuppyDebugWindowComponent> message)
        {
            Settings.IsDebugWindowEnabled.Value = !Settings.IsDebugWindowEnabled.Value;
        }

        public void Process(in Guid messageId, Toggle<TerminalWindowComponent> message)
        {
            Settings.IsTerminalWindowEnabled.Value = !Settings.IsTerminalWindowEnabled.Value;
        }
    }
}
