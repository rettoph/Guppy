using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Guppy.GUI.Styles.Values
{
    public sealed class StyleColorValue : StyleValue
    {
        public readonly ImGuiCol Col;
        public readonly Vector4 Value;

        public StyleColorValue(ImGuiCol col, Color value)
        {
            Col = col;
            Value = value.ToVector4();
        }

        public override void Pop()
        {
            ImGui.PopStyleColor();
        }

        public override void Push()
        {
            ImGui.PushStyleColor(Col, Value);
        }
    }
}
