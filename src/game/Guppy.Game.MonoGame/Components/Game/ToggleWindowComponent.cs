using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Components;
using Guppy.Game.Input.Common;
using Guppy.Game.MonoGame.Components.Scene;
using Guppy.Game.MonoGame.Messages;

namespace Guppy.Game.MonoGame.Components.Game
{
    [AutoLoad]
    internal class ToggleWindowComponent : EngineComponent, IInputSubscriber<Toggle<SceneDebugWindowComponent>>, IInputSubscriber<Toggle<TerminalWindowComponent>>
    {
        public void Process(in Guid messageId, Toggle<SceneDebugWindowComponent> message)
        {
            Common.Settings.IsDebugWindowEnabled.Value = !Common.Settings.IsDebugWindowEnabled.Value;
        }

        public void Process(in Guid messageId, Toggle<TerminalWindowComponent> message)
        {
            Common.Settings.IsTerminalWindowEnabled.Value = !Common.Settings.IsTerminalWindowEnabled.Value;
        }
    }
}
