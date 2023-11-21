namespace Guppy.GUI.Styling.StylerValues
{
    internal class StylerColorValue : IStylerValue
    {
        public readonly ImGuiNET.ImGuiCol Property;
        public readonly Num.Vector4 Value;

        public StylerColorValue(ImGuiCol property, Num.Vector4 value)
        {
            Property = ImGuiColConverter.Convert(property);
            Value = value;
        }

        public void Pop()
        {
            ImGuiNET.ImGui.PopStyleColor();
        }

        public void Push()
        {
            ImGuiNET.ImGui.PushStyleColor(Property, Value);
        }
    }
}
