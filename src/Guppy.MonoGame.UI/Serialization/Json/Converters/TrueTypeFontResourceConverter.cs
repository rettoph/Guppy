using Guppy.Common;
using Guppy.MonoGame.UI.Resources;
using Guppy.Resources;
using Guppy.Resources.Serialization.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Serialization.Json.Converters
{
    internal class TrueTypeFontResourceConverter : ResourceConverter<TrueTypeFont, string>
    {
        public override IResource<TrueTypeFont, string> Factory(string name, string json)
        {
            return new TrueTypeFontResource(name, json);
        }
    }
}
