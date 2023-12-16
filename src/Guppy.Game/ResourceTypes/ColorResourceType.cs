using Guppy.Attributes;
using Guppy.Resources;
using Guppy.Resources.ResourceTypes;
using Guppy.Serialization;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Guppy.Game.ResourceTypes
{
    [AutoLoad]
    internal class ColorResourceType : DefaultResourceType<Color>
    {
        public ColorResourceType(IJsonSerializer json) : base(json)
        {
        }
    }
}
