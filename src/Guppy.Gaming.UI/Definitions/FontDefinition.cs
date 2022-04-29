using Guppy.Gaming.Content;
using Guppy.Gaming.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI.Definitions
{
    public abstract class FontDefinition
    {
        public abstract string Key { get; }
        public abstract string TrueTypeFontContentKey { get; }
        public abstract int SizePixels { get; }

        public virtual unsafe Font BuildFont(IContentProvider content, ImGuiIOPtr io)
        {
            var ttf = content.Get<TrueTypeFont>(this.TrueTypeFontContentKey);
            var fontPtr = io.Fonts.AddFontFromMemoryTTF((IntPtr)ttf.Value.GetDataPtr(), ttf.Value.GetDataSize(), this.SizePixels);
            var font = new Font(this.Key, ttf, this.SizePixels, fontPtr);

            return font;
        }
    }
}
