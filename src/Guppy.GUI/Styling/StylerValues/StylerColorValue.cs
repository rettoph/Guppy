using ImGuiNET;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StylerValues
{
    internal class StylerColorValue : IStylerValue
    {
        public readonly ImGuiCol Property;
        public readonly Vector4 Value;

        public StylerColorValue(ImGuiCol property, Vector4 value)
        {
            Property = property;
            Value = value;
        }

        public void Pop()
        {
            ImGui.PopStyleColor();
        }

        public void Push()
        {
            ImGui.PushStyleColor(Property, Value);
        }
    }
}
