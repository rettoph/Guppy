using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Core.Serialization.Common.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Game.AssetTypes
{
    internal class ColorAssetType(IJsonSerializationService json) : DefaultAssetType<Color>(json)
    {
    }
}