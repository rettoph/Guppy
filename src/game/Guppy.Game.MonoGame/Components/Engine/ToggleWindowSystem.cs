using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.Services;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Messages;
using Guppy.Game.MonoGame.Systems.Scene;

namespace Guppy.Game.MonoGame.Systems.Engine
{
    public sealed class ToggleWindowSystem(ISettingService settings) : IEngineSystem, IInputSubscriber<Toggle<SceneDebugWindowSystem>>, IInputSubscriber<Toggle<EngineTerminalWindowSystem>>
    {
        private readonly SettingValue<bool> _isDebugWindowEnabled = settings.GetValue(Common.Settings.IsDebugWindowEnabled);
        private readonly SettingValue<bool> _isTerminalWindowEnabled = settings.GetValue(Common.Settings.IsTerminalWindowEnabled);

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine engine)
        {
            //
        }

        public void Process(in Guid messageId, Toggle<SceneDebugWindowSystem> message)
        {
            this._isDebugWindowEnabled.Value = !this._isDebugWindowEnabled.Value;
        }

        public void Process(in Guid messageId, Toggle<EngineTerminalWindowSystem> message)
        {
            this._isTerminalWindowEnabled.Value = !this._isTerminalWindowEnabled.Value;
        }
    }
}