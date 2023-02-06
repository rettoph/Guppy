using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public class ImGuiFont
    {
        public readonly string Name;
        public readonly TrueTypeFont TrueTypeFontResource;
        public readonly int SizePixels;
        public readonly ImFontPtr Ptr;

        public ImGuiFont(string name, TrueTypeFont trueTypeFontResource, int sizePixels, ImFontPtr ptr)
        {
            this.Name = name;
            this.TrueTypeFontResource = trueTypeFontResource;
            this.SizePixels = sizePixels;
            this.Ptr = ptr;
        }
    }
}
