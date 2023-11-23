using Guppy.GUI.Styling;
using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public partial interface IGui
    {
        ImGuiFont GetFont(Resource<TrueTypeFont> ttf, int size);

        IStyler GetStyler(Resource<Style> style);

        void TextCentered(string text)
        {
            float windowWidth = this.GetWindowWidth();
            float textWidth = this.CalcTextSize(text).X;

            this.SetCursorPosX((windowWidth - textWidth) / 2);
            this.Text(text);
        }
    }
}
