using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Engine.Common;
using Guppy.Engine.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Messages;
using Guppy.Game.Input.Common;

namespace Guppy.Game.ImGui.MonoGame.Systems.Engine
{
    public class ImGuiEventSystem(IImguiBatch imGuiBatch) : IEngineSystem, IInputSubscriber<ImGuiKeyEvent>, IInputSubscriber<ImGuiMouseButtonEvent>
    {
        private readonly IImguiBatch _imGuiBatch = imGuiBatch;

        [SequenceGroup<InitializeSystemSequenceGroupEnum>(InitializeSystemSequenceGroupEnum.Initialize)]
        public void Initialize(IGuppyEngine obj)
        {
            // throw new NotImplementedException();
        }

        public void Process(in Guid messageId, ImGuiKeyEvent message)
        {
            this._imGuiBatch.SetKeyState(message);
        }

        public void Process(in Guid messageId, ImGuiMouseButtonEvent message)
        {
            this._imGuiBatch.SetMouseButtonState(message);
        }
    }
}
