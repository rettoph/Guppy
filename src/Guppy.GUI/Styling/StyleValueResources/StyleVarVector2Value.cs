using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using ImGuiNET;
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
        public readonly ImGuiStyleVar Property;
        public readonly Vector2 Value;

        public StyleVarVector2Value(ImGuiStyleVar var, Vector2 value)
        {
            Property = var;
            Value = value;
        }

        public void Pop()
        {
            ImGui.PopStyleVar();
        }

        public void Push()
        {
            ImGui.PushStyleVar(Property, Value);
        }

        internal override IStylerValue GetStylerValue(ImGuiBatch batcher, IResourceProvider resources)
        {
            return this;
        }
    }
}
