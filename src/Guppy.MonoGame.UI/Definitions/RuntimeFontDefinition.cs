using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Definitions
{
    internal sealed class RuntimeImGuiFontDefinition : ImGuiFontDefinition
    {
        public override string Key { get; }

        public override string TrueTypeFontResourceName { get; }

        public override int SizePixels { get; }

        public RuntimeImGuiFontDefinition(string key, string trueTypeFontResourceName, int sizePixels)
        {
            this.Key = key;
            this.TrueTypeFontResourceName = trueTypeFontResourceName;
            this.SizePixels = sizePixels;
        }
    }
}
