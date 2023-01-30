using Guppy.MonoGame.Resources;
using Guppy.Resources;
using Guppy.Resources.Serialization.Json.Converters;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Serialization.Json.Converters
{
    internal sealed class ColorResourceConverter : ResourceConverter<Color, Color>
    {
        public override IResource<Color, Color> Factory(string name, Color json)
        {
            return new ColorResource(name, json);
        }
    }
}
