using Guppy.Gaming.Definitions;
using Guppy.Providers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Providers
{
    internal sealed class ColorProvider : ResourceProvider<Color>, IColorProvider
    {
        private Dictionary<string, Color> _colors;

        public ColorProvider(IEnumerable<ColorDefinition> definitions)
        {
            _colors = definitions.Select(x => x.BuildColor()).ToDictionary();
        }

        public override bool TryGet(string key, [MaybeNullWhen(false)] out Color resource)
        {
            return _colors.TryGetValue(key, out resource);
        }

        public override IEnumerator<Color> GetEnumerator()
        {
            return _colors.Values.GetEnumerator();
        }
    }
}
