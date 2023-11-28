using Guppy.GUI;
using Guppy.GUI.Helpers;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Vector2))]
    internal sealed class StyleVarVector2Value : StyleValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly Num.Vector2 Value;

        public StyleVarVector2Value(GuiStyleVar var, Vector2 value)
        {
            Property = GuiStyleVarConverter.ConvertToImGui(var);
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
