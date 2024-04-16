using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.ImGui.MonoGame.Common.Messages
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
