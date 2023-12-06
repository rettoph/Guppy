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

[assembly: InternalsVisibleTo("Guppy.Game.MonoGame")]

namespace Guppy.Game.ImGui
{

    public partial interface IImGui
    {
        Ref<ImFontPtr> GetFont(Resource<TrueTypeFont> ttf, int size);

        ResourceValue<ImStyle> GetStyle(Resource<ImStyle> style);

        ImStyle Apply(ImStyle style)
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

        bool Button(string id, string label)
        {
            this.PushID(id);
            bool result = this.Button(label);
            this.PopID();

            return result;
        }
    }
}
