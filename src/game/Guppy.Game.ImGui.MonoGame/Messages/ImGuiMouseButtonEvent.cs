using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.ImGui.MonoGame.Common.Messages
{
    internal sealed class ImGuiMouseButtonEvent(int button, bool down) : Message<ImGuiMouseButtonEvent>, IInput
    {
        public readonly int Button = button;
        public readonly bool Down = down;
    }
}
