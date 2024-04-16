using Guppy.Game.Input.Common;
using Guppy.Core.Messaging.Common;

namespace Guppy.Game.ImGui.Common.Messages
{
    internal sealed class ImGuiKeyEvent : Message<ImGuiKeyEvent>, IInput
    {
        public readonly ImGuiNET.ImGuiKey Key;
        public readonly bool Down;

        public ImGuiKeyEvent(ImGuiKey key, bool down)
        {
            Key = ImGuiKeyConverter.ConvertToImGui(key);
            Down = down;
        }
    }
}
