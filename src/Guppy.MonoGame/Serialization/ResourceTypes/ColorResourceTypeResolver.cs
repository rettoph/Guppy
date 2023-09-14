using Guppy.Attributes;
using Guppy.Resources;
using Guppy.Resources.Serialization.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Serialization.ResourceTypes
{
    [AutoLoad]
    internal class ColorResourceTypeResolver : ResourceTypeResolver<Color>
    {
        private readonly Regex rgbaArrayRegex = new Regex("^(\\d{1,3}),(\\d{1,3}),(\\d{1,3}),(\\d{1,3})$");

        protected override bool TryResolve(Resource<Color> resource, string input, out Color value)
        {
            Match rgbaArray = rgbaArrayRegex.Match(input);
            if(rgbaArray.Success)
            {
                value = new Color(
                    r: byte.Parse(rgbaArray.Groups[1].Value),
                    g: byte.Parse(rgbaArray.Groups[2].Value),
                    b: byte.Parse(rgbaArray.Groups[3].Value),
                    alpha: byte.Parse(rgbaArray.Groups[4].Value)
                );

                return true;
            }

            value = default!;
            return false;
        }
    }
}
