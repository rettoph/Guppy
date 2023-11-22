using Guppy.GUI;
using Guppy.GUI.Helpers;
using Guppy.GUI.Styling.StylerValues;
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
    internal sealed class StyleVarVector2Value : StyleValue, IStylerValue
    {
        public readonly ImGuiNET.ImGuiStyleVar Property;
        public readonly Num.Vector2 Value;

        public StyleVarVector2Value(GuiStyleVar var, Vector2 value)
        {
            Property = GuiStyleVarConverter.ConvertToImGui(var);
            Value = NumericsHelper.Convert(value);
        }

        public void Pop()
        {
            ImGuiNET.ImGui.PopStyleVar();
        }

        public void Push()
        {
            ImGuiNET.ImGui.PushStyleVar(Property, Value);
        }

        internal override IStylerValue GetStylerValue(IGui imgui, IResourceProvider resources)
        {
            return this;
        }
    }
}
