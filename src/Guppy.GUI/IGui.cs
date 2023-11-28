using System.Runtime.CompilerServices;
using Guppy.GUI.Styling;
using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.Common;

[assembly: InternalsVisibleTo("Guppy.MonoGame")]

namespace Guppy.GUI
{

    public partial interface IGui
    {
        Ref<GuiFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size);

        Style GetStyle(Resource<Style> style);

        Style Apply(Style style)
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
