using Guppy.Game.Input;
using Guppy.Core.Messaging;

namespace Guppy.Game.ImGui.Messages
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
