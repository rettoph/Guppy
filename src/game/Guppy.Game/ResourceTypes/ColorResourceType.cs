using Guppy.Core.Common.Attributes;
using Guppy.Core.Resources.ResourceTypes;
using Microsoft.Xna.Framework;
using Guppy.Core.Serialization.Common.Services;

namespace Guppy.Game.ResourceTypes
{
    [AutoLoad]
    internal class ColorResourceType : DefaultResourceType<Color>
    {
        public ColorResourceType(IJsonSerializationService json) : base(json)
        {
        }
    }
}
