using Guppy.Gaming.Content;
using Guppy.Gaming.Providers;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI
{
    public class Font : Resource<ImFontPtr>
    {
        public Content<TrueTypeFont> TrueTypeFont { get; }
        public int SizePixels { get; }

        public Font(string key, Content<TrueTypeFont> trueTypeFont, int sizePixels, ImFontPtr defaultValue) : base(key, defaultValue)
        {
            this.TrueTypeFont = trueTypeFont;
            this.SizePixels = sizePixels;
        }

        public override string? Export()
        {
            throw new NotImplementedException();
        }

        public override void Import(string? value)
        {
            throw new NotImplementedException();
        }
    }
}
