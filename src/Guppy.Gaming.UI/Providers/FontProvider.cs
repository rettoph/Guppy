using Guppy.Gaming.Content;
using Guppy.Gaming.Providers;
using Guppy.Gaming.UI.Definitions;
using Guppy.Providers;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI.Providers
{
    internal sealed class FontProvider : ResourceProvider<Font>, IFontProvider
    {
        private IContentProvider _content;
        private Dictionary<string, Font> _fonts;

        public unsafe FontProvider(
            IContentProvider content,
            ImGuiIOPtr io,
            IEnumerable<FontDefinition> definitions)
        {
            _content = content;

            _fonts = definitions.ToDictionary(x => x.Key, x => x.BuildFont(_content, io));
        }

        public override IEnumerator<Font> GetEnumerator()
        {
            return _fonts.Values.GetEnumerator();
        }

        public override bool TryGet(string key, [MaybeNullWhen(false)] out Font resource)
        {
            return _fonts.TryGetValue(key, out resource);
        }
    }
}
