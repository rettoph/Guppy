using Guppy.Resources;
using System.Runtime.InteropServices;

namespace Guppy.Game.ImGui
{
    [StructLayout(LayoutKind.Auto)]
    public partial struct ImFontPtr
    {
        public readonly ResourceValue<TrueTypeFont> TTF;
        public readonly int Size;

        internal ImFontPtr(ResourceValue<TrueTypeFont> ttf, int size) : this(default!)
        {
            TTF = ttf;
            Size = size;
        }

        internal void SetImFontPtr(ImGuiNET.ImFontAtlasPtr atlas)
        {
            IntPtr ptr = this.TTF.Value.GetDataPtr();
            this.Value = atlas.AddFontFromMemoryTTF(ptr, this.TTF.Value.GetDataSize(), this.Size);
        }
    }
}
