﻿using Guppy.Core.Resources.Common;
using System.Runtime.InteropServices;

namespace Guppy.Game.ImGui.Common
{
    [StructLayout(LayoutKind.Auto)]
    public partial struct ImFontPtr
    {
        public readonly Resource<TrueTypeFont> TTF;
        public readonly int Size;

        internal ImFontPtr(Resource<TrueTypeFont> ttf, int size) : this(default!)
        {
            TTF = ttf;
            Size = size;
        }

        internal void SetImFontPtr(ImGuiNET.ImFontAtlasPtr atlas)
        {
            IntPtr ptr = this.TTF.Value.GetDataPtr();
            this.Value = atlas.AddFontFromMemoryTTF(ptr, this.TTF.Value.GetDataSize(), this.Size);
        }

        public static bool operator ==(ImFontPtr left, ImFontPtr right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ImFontPtr left, ImFontPtr right)
        {
            return !(left == right);
        }
    }
}
