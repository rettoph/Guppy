using Guppy.Core.Common;
using Guppy.Core.Serialization.Common.Attributes;

namespace Guppy.Game.ImGui.Common.Styling.StyleValues
{
    [PolymorphicJsonType<ImStyleValue>(nameof(TrueTypeFont))]
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
