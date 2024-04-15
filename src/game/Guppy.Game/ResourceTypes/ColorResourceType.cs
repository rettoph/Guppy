using Guppy.Engine.Attributes;
using Guppy.Core.Resources.ResourceTypes;
using Guppy.Engine.Serialization;
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
