using Guppy.GUI.Styling.StylerValues;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Single))]
    internal sealed class StyleVarFloatValue : StyleValue, IStylerValue
    {
        public readonly ImGuiStyleVar Property;
        public readonly float Value;

        public StyleVarFloatValue(ImGuiStyleVar property, float value)
        {
            Property = property;
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
