using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Single))]
    internal sealed class StyleVarFloatValue : StyleValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly float Value;

        public StyleVarFloatValue(GuiStyleVar property, float value)
        {
            Property = GuiStyleVarConverter.ConvertToImGui(property);
            Value = value;
        }

        public override void Pop()
        {
            ImGuiNET.ImGui.PopStyleVar();
        }

        public override void Push()
        {
            ImGuiNET.ImGui.PushStyleVar(Property, Value);
        }
    }
}
