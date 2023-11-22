using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    public struct ImGuiFont
    {
        public readonly Resource<TrueTypeFont> TTF;
        public readonly int Size;
        internal readonly ImFontPtr ImFontPtr;

        internal ImGuiFont(Resource<TrueTypeFont> ttf, int size, ImFontPtr imFontPtr)
        {
            TTF = ttf;
            Size = size;
            ImFontPtr = imFontPtr;
        }
    }
}
