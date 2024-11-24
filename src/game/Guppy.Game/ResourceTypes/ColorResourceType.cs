using Guppy.Core.Resources.Common.ResourceTypes;
using Guppy.Core.Serialization.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.ResourceTypes
{
    internal class ColorResourceType(IJsonSerializationService json) : DefaultResourceType<Color>(json)
    {
    }
}
