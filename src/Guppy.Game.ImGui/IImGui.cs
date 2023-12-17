using System.Runtime.CompilerServices;
using Guppy.Game.ImGui.Styling;
using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common;
using System.Drawing;
using Microsoft.Xna.Framework;

[assembly: InternalsVisibleTo("Guppy.Game.MonoGame")]

namespace Guppy.Game.ImGui
{

    public partial interface IImGui
    {
        Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size);

        ResourceValue<ImStyle> GetStyle(Resource<ImStyle> style);

        IDisposable Apply(Resource<ImStyle> style);
        IDisposable Apply(ImStyle style);
        IDisposable Apply(string key);

        IDisposable ApplyID(string id);

        bool CollapsingHeader(string label, Vector4? color)
        {
            if(color is null)
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
