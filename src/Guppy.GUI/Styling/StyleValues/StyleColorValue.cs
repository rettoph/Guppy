using Guppy.Common;
using Guppy.GUI.Helpers;
using Guppy.Resources;
using Guppy.Resources.Attributes;
using Guppy.Resources.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace Guppy.GUI.Styling.StyleValueResources
{
    [PolymorphicJsonType(nameof(Microsoft.Xna.Framework.Color))]
    internal sealed class StyleColorValue : StyleValue
    {
        private Color _colorValue;
        private Num.Vector4 _value;

        public readonly ImGuiCol Property;
        public readonly Ref<Color> Color;

        public StyleColorValue(GuiCol col, Ref<Color> color)
        {
            this.Property = GuiColConverter.ConvertToImGui(col);
            this.Color = color;
        }

        public override void Pop()
        {
            ImGui.PopStyleColor();
        }

        public override void Push()
        {
            if(_colorValue != this.Color.Value)
            {
                _value = NumericsHelper.Convert(this.Color.Value);
                _colorValue = this.Color.Value;
            }

            ImGui.PushStyleColor(this.Property, _value);
        }
    }
}
