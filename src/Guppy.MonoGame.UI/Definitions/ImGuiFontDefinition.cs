﻿using Guppy.Resources.Providers;
using ImGuiNET;

namespace Guppy.MonoGame.UI.Definitions
{
    public abstract class ImGuiFontDefinition : IImGuiFontDefinition
    {
        public abstract string Key { get; }
        public abstract string TrueTypeFontResourceName { get; }
        public abstract int SizePixels { get; }

        public virtual unsafe ImGuiFont BuildFont(IResourceProvider resources, ImGuiIOPtr io)
        {
            var ttf = resources.Get<TrueTypeFont>(this.TrueTypeFontResourceName);
            var ptr = ttf.Value.GetDataPtr();
            var fontPtr = io.Fonts.AddFontFromMemoryTTF(ptr, ttf.Value.GetDataSize(), this.SizePixels);
            var font = new ImGuiFont(this.Key, ttf.Value, this.SizePixels, fontPtr);

            return font;
        }
    }
}
