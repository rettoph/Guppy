using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;

namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Single))]
    internal sealed class ImStyleVarFloatValue : ImStyleValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly float Value;

        public ImStyleVarFloatValue(ImGuiStyleVar property, float value)
        {
            Property = ImGuiStyleVarConverter.ConvertToImGui(property);
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
