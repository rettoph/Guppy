using Guppy.Engine.Common;
using Guppy.Game.ImGui.Helpers;
using Guppy.Resources.Attributes;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Styling.StyleValueResources
{
    [PolymorphicJsonType<ImStyleValue>(nameof(Microsoft.Xna.Framework.Color))]
    internal sealed class ImStyleColorValue : ImStyleValue
    {
        private Color _colorValue;
        private Num.Vector4 _value;

        public readonly ImGuiNET.ImGuiCol Property;
        public readonly IRef<Color> Color;

        public ImStyleColorValue(string? key, ImGuiCol col, IRef<Color> color) : base(key)
        {
            this.Property = ImGuiColConverter.ConvertToImGui(col);
            this.Color = color;
        }

        public override void Pop()
        {
            ImGuiNet.PopStyleColor();
        }

        public override void Push()
        {
            if (_colorValue != this.Color.Value)
            {
                _value = NumericsHelper.Convert(this.Color.Value);
                _colorValue = this.Color.Value;
            }

            ImGuiNet.PushStyleColor(this.Property, _value);
        }
    }
}
