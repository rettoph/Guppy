using Guppy.Common;
using Guppy.Resources;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
