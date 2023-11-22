using Guppy.GUI.Services;
using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Single))]
    internal sealed class StyleVarFloatValue : StyleValue, IStylerValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly float Value;

        public StyleVarFloatValue(ImGuiStyleVar property, float value)
        {
            Property = ImGuiStyleVarConverter.ConvertToImGui(property);
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

        internal override IStylerValue GetStylerValue(IImGuiService imgui, IResourceProvider resources)
        {
            return this;
        }
    }
}
