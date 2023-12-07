﻿using Guppy.Resources;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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