﻿using System.Runtime.InteropServices;
using Guppy.Core.Resources.Common;

namespace Guppy.Game.ImGui.Common
{
    [StructLayout(LayoutKind.Auto)]
    public partial struct ImFontPtr
    {
        public readonly Resource<TrueTypeFont> TTF;
        public readonly int Size;

        public ImFontPtr(Resource<TrueTypeFont> ttf, int size) : this(default!)
        {
            this.TTF = ttf;
            this.Size = size;
        }

        public void SetImFontPtr(ImGuiNET.ImFontAtlasPtr atlas)
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