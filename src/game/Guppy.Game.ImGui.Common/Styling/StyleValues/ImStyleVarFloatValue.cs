using Guppy.Core.Serialization.Common.Attributes;

namespace Guppy.Game.ImGui.Common.Styling.StyleValues
{
    [PolymorphicJsonType<ImStyleValue>(nameof(Single))]
    internal sealed class ImStyleVarFloatValue(string? key, ImGuiStyleVar property, float value) : ImStyleValue(key)
    {
        public readonly ImGuiNET.ImGuiStyleVar Property = ImGuiStyleVarConverter.ConvertToImGui(property);
        public readonly float Value = value;

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
