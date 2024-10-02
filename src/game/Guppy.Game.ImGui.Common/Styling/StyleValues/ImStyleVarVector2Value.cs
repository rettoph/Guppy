using Guppy.Game.ImGui.Common.Helpers;
using Guppy.Core.Serialization.Common.Attributes;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common.Styling.StyleValues
{
    [PolymorphicJsonType<ImStyleValue>(nameof(Vector2))]
    internal sealed class ImStyleVarVector2Value(string? key, ImGuiStyleVar var, Vector2 value) : ImStyleValue(key)
    {
        public readonly ImGuiNET.ImGuiStyleVar Property = ImGuiStyleVarConverter.ConvertToImGui(var);
        public readonly Num.Vector2 Value = NumericsHelper.Convert(value);

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
