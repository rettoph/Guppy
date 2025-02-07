using Guppy.Engine.Common.Systems;
using Guppy.Game.ImGui.Common;
using Guppy.Game.ImGui.Common.Messages;
using Guppy.Game.Input.Common;

namespace Guppy.Game.ImGui.MonoGame.Systems.Engine
{
    public class ImGuiEventSystem(IImguiBatch imGuiBatch) : IEngineSystem, IInputSubscriber<ImGuiKeyEvent>, IInputSubscriber<ImGuiMouseButtonEvent>
    {
        private readonly IImguiBatch _imGuiBatch = imGuiBatch;

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
