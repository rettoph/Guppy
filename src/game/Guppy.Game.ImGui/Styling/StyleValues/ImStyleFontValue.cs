using Guppy.Core.Common;
using Guppy.Core.Serialization.Common.Attributes;

namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    [PolymorphicJsonType<ImStyleValue>(nameof(TrueTypeFont))]
    internal class ImStyleFontValue : ImStyleValue
    {
        public readonly Ref<ImFontPtr> Font;

        public ImStyleFontValue(string? key, Ref<ImFontPtr> font) : base(key)
        {
            this.Font = font;
        }

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
