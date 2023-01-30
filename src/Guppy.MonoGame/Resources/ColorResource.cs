using Guppy.Resources;
using Guppy.Resources.Attributes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Resources
{
    [PolymorphicJsonType("Color")]
    public class ColorResource : Resource<Color>
    {
        public ColorResource(string name, Color value) : base(name, value)
        {
        }
    }
}
