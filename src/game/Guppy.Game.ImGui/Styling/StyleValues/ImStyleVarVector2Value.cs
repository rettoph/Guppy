using Guppy.Game.ImGui.Helpers;
using Guppy.Resources.Attributes;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    [PolymorphicJsonType<ImStyleValue>(nameof(Vector2))]
    internal sealed class ImStyleVarVector2Value : ImStyleValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly Num.Vector2 Value;

        public ImStyleVarVector2Value(string? key, ImGuiStyleVar var, Vector2 value) : base(key)
        {
            Property = ImGuiStyleVarConverter.ConvertToImGui(var);
            Value = NumericsHelper.Convert(value);
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
