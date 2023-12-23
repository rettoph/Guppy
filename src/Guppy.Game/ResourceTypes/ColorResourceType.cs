using Guppy.Attributes;
using Guppy.Resources.ResourceTypes;
using Guppy.Serialization;
using Microsoft.Xna.Framework;

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
