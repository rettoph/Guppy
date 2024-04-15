using Guppy.Core.Serialization.Common.Attributes;

namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    [PolymorphicJsonType<ImStyleValue>(nameof(Single))]
    internal sealed class ImStyleVarFloatValue : ImStyleValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly float Value;

        public ImStyleVarFloatValue(string? key, ImGuiStyleVar property, float value) : base(key)
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
