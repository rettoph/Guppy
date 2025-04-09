using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Enums;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.Services;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Messages;
using Guppy.Game.MonoGame.Common;
using Guppy.Game.MonoGame.Systems.Scene;

namespace Guppy.Game.MonoGame.Systems.Engine
{
    public sealed class ToggleWindowSystem(
        ISettingService settings
    ) : IEngineSystem,
        IInputSubscriber<Toggle<SceneDebugWindowSystem>>,
        IInputSubscriber<Toggle<EngineTerminalWindowSystem>>
    {
        private readonly SettingValue<bool> _isDebugWindowEnabled = settings.GetValue(GuppyMonoGameSettings.IsDebugWindowEnabled);
        private readonly SettingValue<bool> _isTerminalWindowEnabled = settings.GetValue(GuppyMonoGameSettings.IsTerminalWindowEnabled);

        [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.PreProcess)]
        public void Process(in int messageId, Toggle<SceneDebugWindowSystem> message)
        {
            this._isDebugWindowEnabled.Value = !this._isDebugWindowEnabled.Value;
        }

        [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.PreProcess)]
        public void Process(in int messageId, Toggle<EngineTerminalWindowSystem> message)
        {
            this._isTerminalWindowEnabled.Value = !this._isTerminalWindowEnabled.Value;
        }
    }
}