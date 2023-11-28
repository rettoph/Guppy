using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Single))]
    internal sealed class StyleVarFloatValue : StyleValue, IGuiStyleValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly float Value;

        public StyleVarFloatValue(GuiStyleVar property, float value)
        {
            Property = GuiStyleVarConverter.ConvertToImGui(property);
            Value = value;
        }

        public void Pop()
        {
            ImGuiNET.ImGui.PopStyleVar();
        }

        public void Push()
        {
            ImGuiNET.ImGui.PushStyleVar(Property, Value);
        }

        internal override IGuiStyleValue GetGuiStyleValue(IGui imgui, IResourceProvider resources)
        {
            return this;
        }
    }
}
