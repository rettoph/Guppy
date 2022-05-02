using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.UI.Definitions.FontDefinitions
{
    internal sealed class RuntimeFontDefinition : FontDefinition
    {
        public override string Key { get; }

        public override string TrueTypeFontContentKey { get; }

        public override int SizePixels { get; }

        public RuntimeFontDefinition(string key, string trueTypeFontContentKey, int sizePixels)
        {
            this.Key = key;
            this.TrueTypeFontContentKey = trueTypeFontContentKey;
            this.SizePixels = sizePixels;
        }
    }
}
