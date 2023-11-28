using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    [StructLayout(LayoutKind.Auto)]
    public partial struct GuiFontPtr
    {
        public readonly Resource<TrueTypeFont> TTF;
        public readonly int Size;

        internal GuiFontPtr(Resource<TrueTypeFont> ttf, int size, ImFontPtr value) : this(value)
        {
            TTF = ttf;
            Size = size;
        }
    }
}
