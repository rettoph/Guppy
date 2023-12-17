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

        ResourceValue<Styling.ImStyle> GetStyle(Resource<Styling.ImStyle> style);

        IDisposable Apply(Resource<Styling.ImStyle> style);
        IDisposable Apply(Styling.ImStyle style);
        IDisposable Apply(string key);

        void ObjectViewer(object instance, string? filter = null, int maxDepth = 5, int currentDepth = 0);
        bool ObjectViewer(int? index, string? name, Type type, object? instance, string? filter, int maxDepth, int currentDepth);

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
