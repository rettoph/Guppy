using Guppy.Game.Input;
using Guppy.Messaging;

namespace Guppy.Game.ImGui.Messages
{
    internal sealed class ImGuiMouseButtonEvent : Message<ImGuiMouseButtonEvent>, IInput
    {
        public readonly int Button;
        public readonly bool Down;

        public ImGuiMouseButtonEvent(int button, bool down)
        {
            this.Button = button;
            this.Down = down;
        }
    }
}
