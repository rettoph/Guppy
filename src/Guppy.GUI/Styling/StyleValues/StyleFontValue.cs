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

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(TrueTypeFont))]
    internal class StyleFontValue : StyleValue
    {
        public readonly Ref<GuiFontPtr> Font;

        public StyleFontValue(Ref<GuiFontPtr> font)
        {
            this.Font = font;
        }

        public override void Pop()
        {
            ImGui.PopFont();
        }

        public override void Push()
        {
            ImGui.PushFont(this.Font.Value.Value);
        }
    }
}
