using Guppy.Core.Common;
using Guppy.Game.ImGui.Common.Helpers;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common.Styling.StyleValues
{
    internal sealed class ImStyleColorValue(string? key, ImGuiCol col, IRef<Color> color) : ImStyleValue(key)
    {
        private Color _colorValue;
        private Num.Vector4 _value;

        public readonly ImGuiNET.ImGuiCol Property = ImGuiColConverter.ConvertToImGui(col);
        public readonly IRef<Color> Color = color;

        public override void Pop()
        {
            ImGuiNet.PopStyleColor();
        }

        public override void Push()
        {
            if (this._colorValue != this.Color.Value)
            {
                this._value = NumericsHelper.Convert(this.Color.Value);
                this._colorValue = this.Color.Value;
            }

            ImGuiNet.PushStyleColor(this.Property, this._value);
        }
    }
}