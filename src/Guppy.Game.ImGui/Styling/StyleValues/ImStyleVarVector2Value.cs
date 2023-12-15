using Guppy.Game.ImGui;
using Guppy.Game.ImGui.Helpers;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Vector2))]
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
