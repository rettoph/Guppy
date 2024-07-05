using Guppy.Core.Common;
using Guppy.Core.Resources.Common;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Guppy.Game")]

namespace Guppy.Game.ImGui.Common
{

    public partial interface IImGui
    {
        Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size);

        IDisposable Apply(Resource<ImStyle> style);
        IDisposable Apply(ResourceValue<ImStyle> style);
        IDisposable Apply(ImStyle style);
        IDisposable Apply(string key);

        IDisposable ApplyID(string id);

        bool CollapsingHeader(string label, Vector4? color)
        {
            if (color is null)
            {
                return this.CollapsingHeader(label);
            }

            this.PushStyleColor(ImGuiCol.Header, color.Value);
            bool result = this.CollapsingHeader(label);
            this.PopStyleColor();

            return result;
        }

        void TextCentered(string text)
        {
            float windowWidth = this.GetWindowWidth();
            float textWidth = this.CalcTextSize(text).X;

            this.SetCursorPosX((windowWidth - textWidth) / 2);
            this.Text(text);
        }

        bool Button(string id, string label)
        {
            this.PushID(id);
            bool result = this.Button(label);
            this.PopID();

            return result;
        }
    }
}
