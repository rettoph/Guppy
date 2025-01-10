using Guppy.Core.Messaging.Common;
using Guppy.Game.Input.Common;

namespace Guppy.Game.ImGui.Common.Messages
{
    public sealed class ImGuiMouseButtonEvent(int button, bool down) : Message<ImGuiMouseButtonEvent>, IInput
    {
        public readonly int Button = button;
        public readonly bool Down = down;
    }
}