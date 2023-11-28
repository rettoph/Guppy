namespace Guppy.GUI.Styling.StylerValues
{
    internal class GuiStyleColorValue : IGuiStyleValue
    {
        public readonly ImGuiNET.ImGuiCol Property;
        public readonly Num.Vector4 Value;

        public GuiStyleColorValue(GuiCol property, Num.Vector4 value)
        {
            Property = GuiColConverter.ConvertToImGui(property);
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
