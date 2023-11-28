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
        GuiFontPtr GetFont(Resource<TrueTypeFont> ttf, int size);

        IGuiStyle GetStyle(Resource<Style> style);

        IGuiStyle Apply(IGuiStyle style)
        {
            style.Push();

            return style;
        }

        void TextCentered(string text)
        {
            float windowWidth = this.GetWindowWidth();
            float textWidth = this.CalcTextSize(text).X;

            this.SetCursorPosX((windowWidth - textWidth) / 2);
            this.Text(text);
        }
    }
}
