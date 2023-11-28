using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StylerValues
{
    internal class GuiStyleFontValue : IGuiStyleValue
    {
        public readonly ImFontPtr Font;

        public GuiStyleFontValue(ImFontPtr font)
        {
            Font = font;
        }

        public void Pop()
        {
            ImGui.PopFont();
        }

        public void Push()
        {
            ImGui.PushFont(Font);
        }
    }
}
