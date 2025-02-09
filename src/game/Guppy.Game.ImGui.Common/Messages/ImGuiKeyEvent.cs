using Guppy.Game.Input.Common;

namespace Guppy.Game.ImGui.Common.Messages
{
    public sealed class ImGuiKeyEvent(ImGuiKey key, bool down) : InputMessage<ImGuiKeyEvent>, IInputMessage
    {
        public readonly ImGuiNET.ImGuiKey Key = ImGuiKeyConverter.ConvertToImGui(key);
        public readonly bool Down = down;
    }
}