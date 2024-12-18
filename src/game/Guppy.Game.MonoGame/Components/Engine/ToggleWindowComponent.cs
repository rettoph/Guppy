﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Components;
using Guppy.Engine.Common.Enums;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Messages;
using Guppy.Game.MonoGame.Components.Scene;

namespace Guppy.Game.MonoGame.Components.Engine
{
    internal sealed class ToggleWindowComponent(ISettingService settings) : IEngineComponent, IInputSubscriber<Toggle<SceneDebugWindowComponent>>, IInputSubscriber<Toggle<EngineTerminalWindowComponent>>
    {
        private readonly SettingValue<bool> _isDebugWindowEnabled = settings.GetValue(Common.Settings.IsDebugWindowEnabled);
        private readonly SettingValue<bool> _isTerminalWindowEnabled = settings.GetValue(Common.Settings.IsTerminalWindowEnabled);

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        public void Process(in Guid messageId, Toggle<SceneDebugWindowComponent> message)
        {
            _isDebugWindowEnabled.Value = !_isDebugWindowEnabled.Value;
        }

        public void Process(in Guid messageId, Toggle<EngineTerminalWindowComponent> message)
        {
            _isTerminalWindowEnabled.Value = !_isTerminalWindowEnabled.Value;
        }
    }
}
