using System.Runtime.CompilerServices;
using Guppy.Core.Common;
using Guppy.Core.Assets.Common;
using Guppy.Game.ImGui.Common.Styling;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ImGui.Common
{

    public partial interface IImGui
    {
        Ref<ImFontPtr> GetFont(AssetKey<TrueTypeFont> ttf, int size);

        IDisposable Apply(AssetKey<ImStyle> style);
        IDisposable Apply(Asset<ImStyle> style);
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