using Guppy.Core.Common;

namespace Guppy.Game.ImGui.Common.Styling.StyleValues
{
    internal class ImStyleFontValue(string? key, Ref<ImFontPtr> font) : ImStyleValue(key)
    {
        public readonly Ref<ImFontPtr> Font = font;

        public override void Pop()
        {
            ImGuiNet.PopFont();
        }

        public override void Push()
        {
            ImGuiNet.PushFont(this.Font.Value.Value);
        }
    }
}
